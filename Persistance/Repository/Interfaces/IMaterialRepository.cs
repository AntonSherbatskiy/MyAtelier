using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IMaterialRepository : IRepository<int, Material>
{
    Task<Material?> GetMaterialByNameAsync(string name);
    Task<Material?> GetMaterialByIdAsync(int commandId);
    Task<IEnumerable<Material>> GetMaterialsIncludedAllAsync();
}