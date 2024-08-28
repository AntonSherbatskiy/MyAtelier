using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class ServiceAggregatorConfiguration : IEntityTypeConfiguration<ServiceAggregator>
{
    public void Configure(EntityTypeBuilder<ServiceAggregator> builder)
    {
        builder
            .HasKey(sr => sr.Id);

        builder
            .Property(sa => sa.ServiceType)
            .HasColumnType("enum('Repairing', 'Sewing')")
            .IsRequired();
        
        builder
            .HasOne(sa => sa.RepairingService)
            .WithMany()
            .HasForeignKey(rs => rs.RepairingServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(sa => sa.SewingService)
            .WithMany()
            .HasForeignKey(rs => rs.SewingServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(sa => sa.Orders)
            .WithOne(o => o.ServiceAggregator)
            .HasForeignKey(o => o.ServiceAggregatorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}