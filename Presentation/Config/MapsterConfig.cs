using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Application.Models;
using Application.Usecases.Commands.Authentication;
using Application.Usecases.Commands.Clothing;
using Application.Usecases.Commands.Material;
using Application.Usecases.Commands.MaterialType;
using Application.Usecases.Commands.Order;
using Application.Usecases.Commands.Services.Repairing;
using Application.Usecases.Commands.Services.Sewing;
using Application.Usecases.Commands.User;
using MyAtelier.DAL.Entities;
using AddClothingRequest = Presentation.Contracts.Clothing.AddClothingRequest;
using AddMaterialRequest = Presentation.Contracts.Material.AddMaterialRequest;
using AddMaterialTypeRequest = Presentation.Contracts.MaterialType.AddMaterialTypeRequest;
using AddRepairingServiceRequest = Presentation.Contracts.Services.Repairing.AddRepairingServiceRequest;
using AddSewingServiceRequest = Presentation.Contracts.Services.Sewing.AddSewingServiceRequest;
using Authentication_RegisterRequest = Presentation.Contracts.Authentication.RegisterRequest;
using Clothing_AddClothingRequest = Presentation.Contracts.Clothing.AddClothingRequest;
using Material_AddMaterialRequest = Presentation.Contracts.Material.AddMaterialRequest;
using Orders_PlaceOrderRequest = Presentation.Contracts.Orders.PlaceOrderRequest;
using Orders_UpdateOrderStatusRequest = Presentation.Contracts.Orders.UpdateOrderStatusRequest;
using PlaceOrderRequest = Presentation.Contracts.Orders.PlaceOrderRequest;
using RegisterRequest = Presentation.Contracts.Authentication.RegisterRequest;
using Repairing_AddRepairingServiceRequest = Presentation.Contracts.Services.Repairing.AddRepairingServiceRequest;
using Sewing_AddSewingServiceRequest = Presentation.Contracts.Services.Sewing.AddSewingServiceRequest;
using UpdateOrderStatusRequest = Presentation.Contracts.Orders.UpdateOrderStatusRequest;

namespace Presentation.Config;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        AddUserConfig(config);
        AddRoleConfig(config);
        AddClothingConfig(config);
        AddMaterialConfig(config);
        AddRepairingServiceConfig(config);
        AddServiceAggregatorConfig(config);
        AddSewingServiceConfig(config);
        AddRegisterConfig(config);
        AddOrderConfig(config);
        AddConfirmationCodeConfig(config);
    }

    private void AddUserConfig(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterCommand, ApplicationUser>()
            .Map(command => command.FirstName, user => user.FirstName)
            .Map(command => command.LastName, user => user.LastName)
            .Map(command => command.Email, user => user.Email)
            .Map(user => user.UserName, command => command.Email)
            .PreserveReference(true);

        config.NewConfig<UserModel?, ApplicationUser>()
            .Map(user => user.UserName, model => model.Email)
            .Map(user => user.Email, model => model.Email)
            .Map(user => user.FirstName, model => model.FirstName)
            .Map(user => user.LastName, model => model.LastName)
            .PreserveReference(true);

        config.NewConfig<ApplicationUser, UserModel>()
            .Map(model => model.FirstName, user => user.FirstName)
            .Map(model => model.LastName, user => user.LastName)
            .Map(model => model.Email, user => user.Email)
            .PreserveReference(true);

        config.NewConfig<(string token, UserModel user), AuthenticationResult>()
            .Map(res => res.Token, tuple => tuple.token)
            .Map(res => res.UserModel, tuple => tuple.user)
            .PreserveReference(true);
        
        config.NewConfig<ApplicationUser, UpdateUserModel>()
            .Map(model => model.FirstName, user => user.FirstName)
            .Map(model => model.LastName, user => user.LastName)
            .Map(model => model.Email, user => user.Email)
            .Map(model => model.Password, "")
            .Map(model => model.Id, user => user.Id)
            .PreserveReference(true);

        config.NewConfig<(ApplicationUser user, string roleName), UserModel>()
            .Map(model => model.FirstName, tuple => tuple.user.FirstName)
            .Map(model => model.LastName, tuple => tuple.user.LastName)
            .Map(model => model.Email, tuple => tuple.user.Email)
            .Map(model => model.RoleName, tuple => tuple.roleName);

        config.NewConfig<(IdentityRole role, AddUserCommand command), UserModel>()
            .Map(model => model.FirstName, tuple => tuple.command.FirstName)
            .Map(model => model.LastName, tuple => tuple.command.LastName)
            .Map(model => model.Email, tuple => tuple.command.Email)
            .Map(model => model.Password, tuple => tuple.command.Password)
            .Map(model => model.RoleName, tuple => tuple.role.Name);
    }

    private void AddRoleConfig(TypeAdapterConfig config)
    {
        // config.NewConfig<Role, RoleModel>()
        //     .Map(rm => rm.Name, r => r.Name)
        //     .Map(rm => rm.Id, r => r.Id);
    }

    private void AddClothingConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Clothing, ClothingModel>()
            .Map(cm => cm.Name, c => c.Name)
            .Map(cm => cm.Id, c => c.Id)
            .PreserveReference(true);
        
        config.NewConfig<Clothing_AddClothingRequest, AddClothingCommand>()
            .Map(command => command.Name, req => req.Name)
            .PreserveReference(true);

        config.NewConfig<AddClothingCommand, Clothing>()
            .Map(c => c.Name, command => command.Name)
            .PreserveReference(true);
        
        config.NewConfig<AddClothingCommand, ClothingModel>()
            .Map(model => model.Name, command => command.Name)
            .PreserveReference(true);

        config.NewConfig<ClothingModel, Clothing>()
            .Map(c => c.Name, cm => cm.Name)
            .PreserveReference(true);
    }

    private void AddMaterialConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Material, MaterialModel>()
            .Map(model => model.Name, material => material.Name)
            .Map(model => model.Quantity, material => material.Quantity)
            .Map(model => model.Id, material => material.Id)
            .PreserveReference(true);
        
        config.NewConfig<AddMaterialCommand, Material>()
            .Map(material => material.Name, command => command.MaterialName)
            .Map(material => material.Quantity, command => command.Quantity)
            .PreserveReference(true);

        config.NewConfig<AddMaterialCommand, Material>()
            .Map(material => material.Name, command => command.MaterialName)
            .Map(material => material.Quantity, command => command.Quantity)
            .PreserveReference(true);

        config.NewConfig<Material_AddMaterialRequest, AddMaterialCommand>()
            .Map(command => command.MaterialName, req => req.MaterialName)
            .Map(command => command.Quantity, req => req.Quantity)
            .PreserveReference(true);
        
        config.NewConfig<AddMaterialCommand, Material>()
            .Map(m => m.Name, command => command.MaterialName)
            .Map(m => m.Quantity, command => command.Quantity)
            .PreserveReference(true);
    }
    
    private void AddRepairingServiceConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Repairing_AddRepairingServiceRequest, AddRepairingServiceCommand>()
            .Map(c => c.Price, r => r.Price)
            .Map(c => c.Description, r => r.Description)
            .Map(c => c.AdditionalInfo, r => r.AdditionalInfo)
            .Map(c => c.ClothingName, r => r.ClothingName)
            .PreserveReference(true);

        config.NewConfig<AddRepairingServiceCommand, RepairingService>()
            .Map(service => service.Price, command => command.Price)
            .Map(service => service.Description, command => command.Description)
            .Map(service => service.EstimatedDays, command => command.EstimatedDays)
            .PreserveReference(true);

        config.NewConfig<(int clothingId, AddRepairingServiceCommand addRepairingServiceCommand), RepairingService>()
            .Map(service => service.Price, tuple => tuple.addRepairingServiceCommand.Price)
            .Map(service => service.Description, tuple => tuple.addRepairingServiceCommand.Description)
            .Map(service => service.EstimatedDays, tuple => tuple.addRepairingServiceCommand.EstimatedDays)
            .Map(service => service.ClothingId, tuple => tuple.clothingId)
            .PreserveReference(true);

        config.NewConfig<RepairingService, RepairingServiceModel>()
            .Map(model => model.Id, service => service.Id)
            .Map(model => model.Description, service => service.Description)
            .Map(model => model.Price, service => service.Price)
            .Map(model => model.ClothingModel.Id, service => service.ClothingId)
            .Map(model => model.ClothingModel.Name, service => service.Clothing.Name)
            .PreserveReference(true);

        config.NewConfig<RepairingServiceModel, PlaceRepairingOrderModel>()
            .Map(order => order.Price, model => model.Price)
            .Map(order => order.Description, model => model.Description)
            .Map(order => order.Id, model => model.Id)
            .Map(order => order.ClothingModel, model => model.ClothingModel)
            .Map(order => order.ServiceName, model => $"{model.ClothingModel.Name} repairing")
            .PreserveReference(true);

        config.NewConfig<PlaceRepairingOrderModel, PlaceOrderCommand>()
            .Map(command => command.AdditionalInfo, model => model.AdditionalInfo)
            .Map(command => command.ServiceId, model => model.Id)
            .Map(command => command.ServiceCategory, model => "Repairing")
            .Map(command => command.ClothingName, model => model.ClothingModel.Name)
            .PreserveReference(true);
    }

    private void AddServiceAggregatorConfig(TypeAdapterConfig config)
    {
        config.NewConfig<RepairingService, ServiceAggregator>()
            .Map(sa => sa.RepairingServiceId, rs => rs.Id)
            .Map(sa => sa.ServiceType, _ => "Repairing")
            .PreserveReference(true);
    }

    private void AddSewingServiceConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Sewing_AddSewingServiceRequest, AddSewingServiceCommand>()
            .Map(command => command.Description, req => req.Description)
            .Map(command => command.EstimatedDays, req => req.EstimatedDays)
            .Map(command => command.MaterialName, req => req.MaterialName)
            .Map(command => command.Price, req => req.Price)
            .Map(command => command.ClothingSize, req => req.Size)
            .Map(command => command.MaterialNeeded, req => req.MaterialNeeded)
            .PreserveReference(true);

        config.NewConfig<(int clothingId, int materialId, AddSewingServiceCommand command), SewingService>()
            .Map(service => service.Description, tuple => tuple.command.Description)
            .Map(service => service.EstimatedDays, tuple => tuple.command.EstimatedDays)
            .Map(service => service.Price, tuple => tuple.command.Price)
            .Map(service => service.ClothingSize, tuple => tuple.command.ClothingSize)
            .Map(service => service.MaterialNeeded, tuple => tuple.command.MaterialNeeded)
            .Map(service => service.ClothingId, tuple => tuple.clothingId)
            .Map(service => service.MaterialId, tuple => tuple.materialId)
            .PreserveReference(true);

        config.NewConfig<SewingService, SewingServiceModel>()
            .Map(model => model.Description, service => service.Description)
            .Map(model => model.Price, service => service.Price)
            .Map(model => model.ClothingSize, service => service.ClothingSize)
            .Map(model => model.MaterialModel, service => service.Material)
            .Map(model => model.ClothingModel, service => service.Clothing)
            .Map(model => model.Id, service => service.Id)
            .Map(model => model.EstimatedDays, service => service.EstimatedDays)
            .PreserveReference(true);

        config.NewConfig<SewingServiceModel, PlaceSewingOrderModel>()
            .Map(order => order.Id, model => model.Id)
            .Map(order => order.EstimatedDays, model => model.EstimatedDays)
            .Map(order => order.MaterialModel, model => model.MaterialModel)
            .Map(order => order.Size, model => model.ClothingSize)
            .Map(order => order.Description, model => model.Description)
            .Map(order => order.Price, model => model.Price)
            .Map(order => order.ClothingModel, model => model.ClothingModel)
            .Map(order => order.ServiceName, model => $"{model.ClothingModel.Name} Sewing")
            .PreserveReference(true);

        config.NewConfig<PlaceSewingOrderModel, PlaceOrderCommand>()
            .Map(command => command.ServiceId, model => model.Id)
            .Map(command => command.AdditionalInfo, model => model.AdditionalInfo)
            .Map(command => command.ServiceCategory, model => "Sewing")
            .Map(command => command.ClothingSize, model => model.Size)
            .Map(command => command.ClothingName, model => model.ClothingModel.Name)
            .Map(command => command.MaterialName, model => model.MaterialModel.Name)
            .PreserveReference(true);
    }

    private void AddRegisterConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Authentication_RegisterRequest, RegisterCommand>()
            .Map(command => command.FirstName, request => request.FirstName)
            .Map(command => command.LastName, request => request.LastName)
            .Map(command => command.Email, request => request.Email)
            .Map(command => command.Password, request => request.Password)
            .Map(command => command.ConfirmedPassword, request => request.ConfirmedPassword)
            .PreserveReference(true);

        config.NewConfig<ConfirmationCodeModel, RegisterCommand>()
            .Map(command => command, model => model)
            .Map(command => command.ConfirmationCode, model => model.Code)
            .Map(command => command.Password, model => model.Password)
            .Map(command => command.ConfirmedPassword, model => model.Password)
            .PreserveReference(true);
    }

    private void AddOrderConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Orders_PlaceOrderRequest, PlaceOrderCommand>()
            .Map(command => command.ServiceId, request => request.ServiceId)
            .Map(command => command.ServiceCategory, request => request.ServiceCategory)
            .PreserveReference(true);

        config.NewConfig<Order, OrderModel>()
            .Map(model => model.Price, order => order.Price)
            .Map(model => model.Status, order => order.Status)
            .Map(model => model.ServiceAggregatorId, order => order.ServiceAggregatorId)
            .Map(model => model.PlacedAt, order => order.PlacedAt)
            .Map(model => model.IsClothesBrought, order => order.IsClothesBrought)
            .Map(model => model.Id, order => order.Id)
            .Map(model => model.AdditionalInfo, order => order.AdditionalInformation)
            .Map(model => model.ClothingSize, order => order.ClothingSize)
            .Map(model => model.ClothingName, order => order.ClothingName)
            .Map(model => model.MaterialName, order => order.MaterialName)
            .PreserveReference(true);

        config.NewConfig<Orders_UpdateOrderStatusRequest, UpdateOrderStatusCommand>()
            .Map(command => command.OrderStatus, request => request.Status)
            .PreserveReference(true);

        config.NewConfig<OrderModel, RepairingOrderDetails>()
            .Map(details => details.OrderModel.AdditionalInfo, model => model.AdditionalInfo)
            .Map(details => details.OrderModel.Price, model => model.Price)
            .Map(details => details.OrderModel.Id, model => model.Id)
            .Map(details => details.OrderModel.PlacedAt, model => model.PlacedAt)
            .Map(details => details.OrderModel.IsClothesBrought, model => model.IsClothesBrought)
            .Map(details => details.OrderModel.Status, model => model.Status)
            .Map(details => details.OrderModel.CompletedAt, model => model.CompletedAt)
            .Map(details => details.OrderModel.ServiceType, model => model.ServiceType)
            .Map(details => details.OrderModel.ServiceAggregatorId, order => order.ServiceAggregatorId)
            .Map(details => details.ClothingName, model => model.ClothingName)
            .PreserveReference(true);
        
        config.NewConfig<OrderModel, SewingOrderDetails>()
            .Map(details => details.OrderModel.AdditionalInfo, order => order.AdditionalInfo)
            .Map(details => details.OrderModel.Price, order => order.Price)
            .Map(details => details.OrderModel.Id, order => order.Id)
            .Map(details => details.OrderModel.PlacedAt, order => order.PlacedAt)
            .Map(details => details.OrderModel.IsClothesBrought, order => order.IsClothesBrought)
            .Map(details => details.OrderModel.Status, order => order.Status)
            .Map(details => details.OrderModel.CompletedAt, order => order.CompletedAt)
            .Map(details => details.OrderModel.ServiceType, order => order.ServiceType)
            .Map(details => details.OrderModel.ServiceAggregatorId, order => order.ServiceAggregatorId)
            .Map(details => details.ClothingName, order => order.ClothingName)
            .Map(details => details.MaterialName, order => order.MaterialName)
            .Map(details => details.ClothingSize, order => order.ClothingSize)
            .PreserveReference(true);
    }

    private void AddConfirmationCodeConfig(TypeAdapterConfig config)
    {
        config.NewConfig<(Authentication_RegisterRequest request, int code), ConfirmationCodeModel>()
            .Map(model => model, tuple => tuple.request)
            .Map(model => model.Code, tuple => tuple.code)
            .PreserveReference(true);
    }
}