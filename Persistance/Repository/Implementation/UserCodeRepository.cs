using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository.Implementation;

public class UserCodeRepository : GenericRepository<UserCode>, IUserCodeRepository
{
    private AppDbContext _context { get; set; }
    
    public UserCodeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserCode?> GetCodeByEmail(string registerCommandEmail)
    {
        return (await _context.UserCodes.ToListAsync()).FindLast(c => c.Email == registerCommandEmail);
    }

    public void RemoveCodesByEmail(string userEmail)
    {
        var codes = _context.UserCodes.Where(c => c.Email == userEmail).ToList();
        _context.UserCodes.RemoveRange(codes);
    }
}