using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
{
    private AppDbContext _context { get; set; }
    
    public MaterialRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Material?> GetMaterialByNameAsync(string name)
    {
        return _context.Materials.FirstOrDefaultAsync(m => m.Name == name);
    }

    public Task<Material?> GetMaterialByIdAsync(int id)
    {
        return _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Material>> GetMaterialsIncludedAllAsync()
    {
        return await _context.Materials.Include(m => m.SewingServices).ToListAsync();
    }
}