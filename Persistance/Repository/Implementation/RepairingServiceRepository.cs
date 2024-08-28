using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class RepairingServiceRepository : GenericRepository<RepairingService>, IRepairingServiceRepository
{
    private AppDbContext _context { get; set; }
    
    public RepairingServiceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public Task<RepairingService?> GetRepairingServiceByClothingNameAsync(string clothingName)
    {
        return _context.RepairingServices
            .Include(s => s.Clothing)
            .FirstOrDefaultAsync(s => s.Clothing.Name == clothingName);
    }

    public async Task<IEnumerable<RepairingService>> GetRepairingServicesByClothingIdAsync(int clothingId)
    {
        return await _context.RepairingServices.Where(r => r.ClothingId == clothingId).ToListAsync();
    }

    public async Task<IEnumerable<RepairingService>> GetIncludedAllAsync()
    {
        return await _context.RepairingServices.Include(r => r.Clothing).ToListAsync();
    }

    public Task<RepairingService?> GetRepairingServiceByIdAsync(int id)
    {
        return _context.RepairingServices.Include(s => s.Clothing).FirstOrDefaultAsync(s => s.Id == id);
    }
}