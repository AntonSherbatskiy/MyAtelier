using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class ServiceAggregatorRepository : GenericRepository<ServiceAggregator>, IServiceAggregatorRepository
{
    private AppDbContext _context { get; set; }
    
    public ServiceAggregatorRepository(AppDbContext context) : base(context)
    { 
        _context = context;
    }

    public Task<ServiceAggregator?> GetServiceAggregatorByServiceId(int serviceId, string serviceType)
    {
        return serviceType == "Repairing" ?
            _context.ServiceAggregators.FirstOrDefaultAsync(s => s.RepairingServiceId == serviceId) : 
            _context.ServiceAggregators.FirstOrDefaultAsync(s => s.SewingServiceId == serviceId);
    }

    public async Task<RepairingService> GetRepairingServiceByIdAsync(int serviceAggregatorId)
    {
        return (await _context.ServiceAggregators
            .Include(s => s.RepairingService.Clothing)
            .FirstOrDefaultAsync(s => s.Id == serviceAggregatorId)).RepairingService;
    }

    public async Task<SewingService> GetSewingServiceByIdAsync(int serviceAggregatorId)
    {
        return (await _context.ServiceAggregators
            .Include(s => s.SewingService.Clothing)
            .Include(s => s.SewingService.Material)
            .FirstOrDefaultAsync(s => s.Id == serviceAggregatorId)).SewingService;
    }
}