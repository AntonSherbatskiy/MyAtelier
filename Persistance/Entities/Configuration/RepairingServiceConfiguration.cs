using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class RepairingServiceConfiguration : IEntityTypeConfiguration<RepairingService>
{
    public void Configure(EntityTypeBuilder<RepairingService> builder)
    {
        builder
            .HasKey(rs => rs.Id);
        
        //TODO Add name and estimated days to sewing service
        //TODO Create logic with repairing and sewing service
        
        builder.Property(rs => rs.EstimatedDays)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(rs => rs.Description)
            .HasColumnType("varchar(500)")
            .IsRequired();
        
        builder
            .HasIndex(r => r.ClothingId)
            .IsUnique();
        
        builder
            .Property(r => r.Price)
            .HasColumnType("double")
            .IsRequired();
        
        builder
            .HasOne(rs => rs.Clothing)
            .WithMany(ct => ct.RepairingServices)
            .HasForeignKey(ct => ct.ClothingId);
    }
}