using System.Collections;
using System.Security.Claims;
using Application.Common.AccountConfirmation;
using Application.ErrorModels;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Order;
using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Implementation;

public class OrderService : IOrderService
{
    private IMapper _mapper { get; set; }
    private IUnitOfWork _unit { get; set; }
    private UserManager<ApplicationUser> _userManager { get; set; }
    private IAsyncMessageSender _messageSender { get; set; }

    public OrderService(IMapper mapper, IUnitOfWork unit, UserManager<ApplicationUser> userManager, IAsyncMessageSender messageSender)
    {
        _mapper = mapper;
        _unit = unit;
        _userManager = userManager;
        _messageSender = messageSender;
    }
    
    public async Task<ErrorOr<OrderModel>> PlaceOrderAsync(PlaceOrderCommand command, string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        return command.ServiceCategory is not ("Sewing" or "Repairing") ? Errors.Order.IncorrectServiceCategory 
            : await AggregateOrderAsync(command, user);
    }

    private async Task<ErrorOr<OrderModel>> AggregateOrderAsync(PlaceOrderCommand command, ApplicationUser user)
    {
        var order = new Order()
        {
            PlacedAt = DateTime.Now,
            UserId = user.Id,
            ClothingName = command.ClothingName,
            Status = "Process"
        };
        
        if (command.ServiceCategory.ToLower() == "repairing")
        {
            var service = await _unit.RepairingServiceRepository.GetRepairingServiceByIdAsync(command.ServiceId);
            
            if (service == null)
            {
                return Errors.Order.IncorrectService;
            }

            var aggregator =
                await _unit.ServiceAggregatorRepository.GetServiceAggregatorByServiceId(command.ServiceId,
                    "Repairing");

            var existedOrder = await _unit.OrderRepository.GetAsync(o => o.UserId == user.Id
                                                                   && o.ServiceAggregatorId == aggregator.Id
                                                                   && o.Status == "Process");

            if (existedOrder != null)
            {
                return Errors.Order.OrderIsAlreadyPlaced;
            }
            
            order.Price = service.Price;
            order.ServiceType = "Repairing";
            order.IsClothesBrought = false;
            order.ServiceAggregatorId = aggregator.Id;
            order.AdditionalInformation = command.AdditionalInfo;
        }
        else
        {
            var service = await _unit.SewingServiceRepository.GetSewingServiceByIdAsync(command.ServiceId);
            
            if (service == null)
            {
                return Errors.Order.IncorrectService;
            }

            var material = await _unit.MaterialRepository.GetMaterialByNameAsync(command.MaterialName);

            if (material.Quantity < service.MaterialNeeded)
            {
                return Errors.Order.NotEnoughMaterial;
            }

            material.Quantity -= service.MaterialNeeded;
            await _unit.MaterialRepository.UpdateAsync(material.Id, material);

            var aggregator =
                await _unit.ServiceAggregatorRepository.GetServiceAggregatorByServiceId(command.ServiceId,
                    "Sewing");
            
            var existedOrder = await _unit.OrderRepository.GetAsync(o => o.UserId == user.Id
                                                                         && o.ServiceAggregatorId == aggregator.Id
                                                                         && o.Status == "Process");

            if (existedOrder != null)
            {
                return Errors.Order.OrderIsAlreadyPlaced;
            }

            order.Price = service.Price;
            order.ServiceType = "Sewing";
            order.IsClothesBrought = null;
            order.ServiceAggregatorId = aggregator.Id;
            order.AdditionalInformation = command.AdditionalInfo;
            order.ClothingSize = command.ClothingSize;
            order.MaterialName = command.MaterialName;
        }
        
        await _unit.OrderRepository.AddAsync(order);
        _unit.Complete();

        return _mapper.Map<OrderModel>(order);
    }

    public async Task<ErrorOr<OrderModel>> UpdateOrderStatus(int orderId, UpdateOrderStatusCommand command)
    {
        if (command.OrderStatus is not ("Canceled" or "Completed"))
        {
            return Errors.Order.IncorrectStatus;
        }
        
        var order = await _unit.OrderRepository.GetOrderByIdAsync(orderId);

        if (order == null)
        {
            return Errors.Order.IncorrectOrder;
        }

        if (order.Status is "Canceled" or "Completed")
        {
            return Errors.Order.OrderFinished;
        }

        order.Status = command.OrderStatus;
        _unit.OrderRepository.Update(order);
        _unit.Complete();
        
        return _mapper.Map<OrderModel>(order);
    }
    
    public async Task<ErrorOr<IEnumerable<OrderModel>>> DeleteOrderByIdAsync(int id)
    {
        var order = await _unit.OrderRepository.GetOrderByIdAsync(id);

        if (order == null)
        {
            return Errors.Order.IncorrectOrder;
        }

        if (order.Status == "Process")
        {
            return Errors.Order.CannotRemoveProcessOrder;
        }

        await _unit.OrderRepository.RemoveAsync(id);
        _unit.Complete();

        var mapped = (await _unit.OrderRepository.GetOrdersIncludedAll()).Select(o 
            => _mapper.Map<OrderModel>(o));

        return (dynamic)mapped;
    }

    public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
    {
        return _mapper.Map<IEnumerable<OrderModel>>(await _unit.OrderRepository.GetOrdersIncludedAll());
    }

    public async Task<IEnumerable<OrderModel>> GetUserOrdersByUserEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        var orders = await _unit.OrderRepository.GetOrdersByUserIdAsync(user!.Id);

        return _mapper.Map<IEnumerable<OrderModel>>(orders);
    }

    public async Task<OrderModel> GetOrderByIdAsync(int id)
    {
        var order = await _unit.OrderRepository.GetOrderByIdAsync(id);
        return _mapper.Map<OrderModel>(order);
    }

    public async Task<ErrorOr<IEnumerable<OrderModel>>> CancelOrderAsync(int id)
    {
        var order = await _unit.OrderRepository.GetOrderByIdAsync(id);

        if (order == null)
        {
            return Errors.Order.IncorrectOrder;
        }

        if (order.Status != "Process")
        {
            return Errors.Order.CannotCancelFinishedOrder;
        }

        order.Status = "Canceled";
        
        await SendEmailMessageAsync(order.User.Email, order.ClothingName, order.ServiceType.ToLower(), "canceled");
        
        await _unit.OrderRepository.UpdateAsync(order.Id, order);
        _unit.Complete();

        var mapped = (await _unit.OrderRepository.GetOrdersIncludedAll()).Select(o 
            => _mapper.Map<OrderModel>(o));

        return (dynamic)mapped;
    }

    public async Task<ErrorOr<IEnumerable<OrderModel>>> CompleteOrder(int id)
    {
        var order = await _unit.OrderRepository.GetOrderByIdAsync(id);

        if (order == null)
        {
            return Errors.Order.IncorrectOrder;
        }

        if (order.Status != "Process")
        {
            return Errors.Order.CannotCompleteFinishedOrder;
        }

        order.Status = "Completed";
        order.CompletedAt = DateTime.Now;
        
        await SendEmailMessageAsync(order.User.Email, order.ClothingName, order.ServiceType.ToLower(), "completed");
        
        await _unit.OrderRepository.UpdateAsync(order.Id, order);
        _unit.Complete();

        var mapped = (await _unit.OrderRepository.GetOrdersIncludedAll()).Select(o 
            => _mapper.Map<OrderModel>(o));

        return (dynamic)mapped;
    }

    private async Task SendEmailMessageAsync(string email, string clothingName, string serviceType, string status)
    {
        await _messageSender.SendAsync(email, "Order info", $"Your order '{clothingName}' {serviceType} was {status}");
    }
}