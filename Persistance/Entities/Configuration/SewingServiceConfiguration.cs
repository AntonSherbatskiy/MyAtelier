using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class SewingServiceConfiguration : IEntityTypeConfiguration<SewingService>
{
    public void Configure(EntityTypeBuilder<SewingService> builder)
    {
        builder
            .HasKey(ss => ss.Id);
        
        builder.Property(s => s.Price)
            .HasColumnType("double")
            .IsRequired();

        builder.Property(s => s.EstimatedDays)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnType("varchar(500)");

        builder.Property(s => s.ClothingSize)
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.Property(s => s.MaterialNeeded)
            .HasColumnType("double")
            .IsRequired();
    }
}