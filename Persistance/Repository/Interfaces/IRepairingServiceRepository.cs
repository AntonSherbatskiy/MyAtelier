using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IRepairingServiceRepository : IRepository<int, RepairingService>
{
    Task<RepairingService?> GetRepairingServiceByClothingNameAsync(string clothingName);
    Task<IEnumerable<RepairingService>> GetRepairingServicesByClothingIdAsync(int clothingId);
    Task<IEnumerable<RepairingService>> GetIncludedAllAsync();
    Task<RepairingService?> GetRepairingServiceByIdAsync(int id);
}