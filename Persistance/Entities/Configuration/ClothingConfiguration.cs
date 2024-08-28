using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class ClothingConfiguration : IEntityTypeConfiguration<Clothing>
{
    public void Configure(EntityTypeBuilder<Clothing> builder)
    {
        builder
            .HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .HasColumnType("varchar(20)")
            .IsRequired();
        
        builder
            .HasMany(ct => ct.RepairingServices)
            .WithOne(rs => rs.Clothing)
            .HasForeignKey(rs => rs.ClothingId);

        builder
            .HasMany(ct => ct.SewingServices)
            .WithOne(s => s.Clothing)
            .HasForeignKey(s => s.ClothingId);
    }
}