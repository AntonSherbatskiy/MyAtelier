using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IOrderRepository : IRepository<int, Order>
{
    Task<Order?> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetOrdersIncludedAll();
    Task<IEnumerable<Order>> GetOrdersByServiceIdAsync(int serviceId, string serviceType);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}