using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class ClothingRepository : GenericRepository<Clothing>, IClothingRepository
{
    private AppDbContext _context { get; set; }
    
    public ClothingRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Clothing?> GetClothingByNameAsync(string name)
    {
        return _context.Clothes.Include(c => c.RepairingServices).Include(c => c.SewingServices).FirstOrDefaultAsync(c => c.Name == name);
    }

    public Task<Clothing?> GetClothingByIdAsync(int id)
    {
        return _context.Clothes.Include(c => c.SewingServices).Include(c => c.RepairingServices)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}