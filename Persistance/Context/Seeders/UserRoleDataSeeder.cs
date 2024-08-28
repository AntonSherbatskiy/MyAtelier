using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Context.Seeders;

public class UserRoleDataSeeder : IDataSeeder<IdentityUserRole<string>>
{
    public void Seed(EntityTypeBuilder<IdentityUserRole<string>> entityTypeBuilder)
    {
        entityTypeBuilder.HasData(new IdentityUserRole<string>
        {
            UserId = "0c5fb748-8514-481e-893f-ab35a604c064",
            RoleId = "84f97938-e8a0-45e4-a952-362765f136fd"
        });
    }
}