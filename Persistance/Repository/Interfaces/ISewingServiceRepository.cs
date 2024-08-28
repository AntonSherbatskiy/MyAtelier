using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface ISewingServiceRepository : IRepository<int, SewingService>
{
    // Task<RepairingService?> GetRepairingServiceByClothingNameAsync(string clothingName);
    // Task<IEnumerable<RepairingService>> GetRepairingServicesByClothingIdAsync(int clothingId);
    // Task<IEnumerable<RepairingService>> GetIncludedAllAsync();

    Task<IEnumerable<SewingService>> GetIncludedAllAsync();
    Task<SewingService?> GetSewingServiceByIdAsync(int id);
}