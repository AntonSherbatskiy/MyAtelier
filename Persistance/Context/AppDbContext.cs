using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context.Seeders;
using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<ServiceAggregator> ServiceAggregators { get; set; }
    public DbSet<SewingService> SewingServices { get; set; }
    public DbSet<RepairingService> RepairingServices { get; set; }
    public DbSet<Clothing> Clothes { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<UserCode> UserCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        new RoleDataSeeder().Seed(modelBuilder.Entity<IdentityRole>());
        new UserDataSeeder().Seed(modelBuilder.Entity<ApplicationUser>());
        new UserRoleDataSeeder().Seed(modelBuilder.Entity<IdentityUserRole<string>>());
    }
}