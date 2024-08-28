using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Context.Seeders;

public class UserDataSeeder : IDataSeeder<ApplicationUser>
{
    public void Seed(EntityTypeBuilder<ApplicationUser> entityTypeBuilder)
    {
        entityTypeBuilder.HasData(
            new ApplicationUser
            {
                Id = "0c5fb748-8514-481e-893f-ab35a604c064",
                UserName = "MyAtelierAdmin",
                NormalizedUserName = "MYATELIERADMIN",
                Email = "adminadmin@admin.com",
                NormalizedEmail = "ADMINADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEEXLzi5AmM5CKN2obmc0CBZ9UYy2LGzInUDyjXAa7l+6TeanWRKFKxCOWPMlBnybPQ==",
                SecurityStamp = null,
                ConcurrencyStamp = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                FirstName = "Admin",
                LastName = "Admin"
            });
    }
}