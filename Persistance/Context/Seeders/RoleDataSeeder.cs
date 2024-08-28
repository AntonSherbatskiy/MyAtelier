using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Context.Seeders;

public class RoleDataSeeder : IDataSeeder<IdentityRole>
{
    public void Seed(EntityTypeBuilder<IdentityRole> entityTypeBuilder)
    {
        entityTypeBuilder.HasData(
            new IdentityRole 
            {
                Id = "b1234e88-3ceb-4383-9409-7775d2e12d3f",
                Name = "user",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = "84f97938-e8a0-45e4-a952-362765f136fd",
                Name = "admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "4291c45b-132f-4901-94b8-0be751948b79",
                Name = "manager",
                NormalizedName = "MANAGER"
            });
    }
}