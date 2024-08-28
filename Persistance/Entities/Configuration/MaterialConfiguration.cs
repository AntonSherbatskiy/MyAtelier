using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder
            .HasKey(m => m.Id);
        
        builder
            .Property(m => m.Name)
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder
            .HasMany(m => m.SewingServices)
            .WithOne(s => s.Material)
            .HasForeignKey(s => s.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}