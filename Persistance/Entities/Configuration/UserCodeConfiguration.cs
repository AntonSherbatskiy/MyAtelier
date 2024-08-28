using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class UserCodeConfiguration : IEntityTypeConfiguration<UserCode>
{
    public void Configure(EntityTypeBuilder<UserCode> builder)
    {
        // builder
        //     .HasOne(uc => uc.ApplicationUser)
        //     .WithOne(App)
        //     .HasForeignKey(uc => uc.)
    }
}