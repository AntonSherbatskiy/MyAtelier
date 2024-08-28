using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class SewingServiceRepository : GenericRepository<SewingService>, ISewingServiceRepository
{
    private AppDbContext _context { get; set; }

    public SewingServiceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SewingService>> GetIncludedAllAsync()
    {
        return await _context.SewingServices
            .Include(s => s.Clothing)
            .Include(s => s.Material)
            .ToListAsync();
    }

    public Task<SewingService?> GetSewingServiceByIdAsync(int id)
    {
        return _context.SewingServices
            .Include(s => s.Material)
            .Include(s => s.Clothing)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}