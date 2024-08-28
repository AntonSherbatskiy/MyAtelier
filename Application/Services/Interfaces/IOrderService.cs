using System.Collections;
using System.Security.Claims;
using Application.Models;
using Application.Usecases.Commands.Order;
using ErrorOr;
using MyAtelier.DAL.Entities;

namespace Application.Services.Interfaces;

public interface IOrderService
{
    Task<ErrorOr<OrderModel>> PlaceOrderAsync(PlaceOrderCommand command, string userEmail);
    Task<ErrorOr<OrderModel>> UpdateOrderStatus(int orderId, UpdateOrderStatusCommand command);
    Task<ErrorOr<IEnumerable<OrderModel>>> DeleteOrderByIdAsync(int id);
    Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
    Task<IEnumerable<OrderModel>> GetUserOrdersByUserEmailAsync(string userEmail);
    Task<OrderModel> GetOrderByIdAsync(int id);
    Task<ErrorOr<IEnumerable<OrderModel>>> CancelOrderAsync(int id);
    Task<ErrorOr<IEnumerable<OrderModel>>> CompleteOrder(int id);
}