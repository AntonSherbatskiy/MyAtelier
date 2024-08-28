using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private AppDbContext _context { get; set; }
    
    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }


    public Task<Order?> GetOrderByIdAsync(int id)
    {
        return _context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersIncludedAll()
    {
        return await _context.Orders.Include(o => o.ServiceAggregator).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByServiceIdAsync(int serviceId, string serviceType)
    {
        return await _context.Orders.Include(o => o.ServiceAggregator).Where(o => o.ServiceType == serviceType)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }
}