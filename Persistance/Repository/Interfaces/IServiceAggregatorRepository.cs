using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IServiceAggregatorRepository : IRepository<int, ServiceAggregator>
{
    Task<ServiceAggregator?> GetServiceAggregatorByServiceId(int serviceId, string serviceType);
    Task<RepairingService> GetRepairingServiceByIdAsync(int serviceAggregatorId);
    Task<SewingService> GetSewingServiceByIdAsync(int serviceAggregatorId);
}