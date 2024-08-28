using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IClothingRepository : IRepository<int, Clothing>
{
    Task<Clothing?> GetClothingByNameAsync(string name);
    Task<Clothing?> GetClothingByIdAsync(int id);
}