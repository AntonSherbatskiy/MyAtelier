using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Entities.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasKey(o => o.Id);
        
        builder.Property(o => o.PlacedAt)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(o => o.CompletedAt)
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(o => o.ClothingName)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(o => o.ClothingSize)
            .HasColumnType("varchar(10)")
            .IsRequired(false);

        builder.Property(o => o.MaterialName)
            .HasColumnType("varchar(50)")
            .IsRequired(false);

        builder.Property(o => o.AdditionalInformation)
            .HasColumnType("varchar(200)")
            .IsRequired(false);

        builder.Property(o => o.Status)
            .HasColumnType("enum('Completed', 'Process', 'Canceled')")
            .IsRequired();

        builder.Property(o => o.IsClothesBrought)
            .HasColumnType("tinyint(1)")
            .IsRequired(false);

        builder.Property(o => o.ServiceType)
            .HasColumnType("enum('Sewing', 'Repairing')")
            .IsRequired();

        builder.Property(o => o.ServiceAggregatorId)
            .IsRequired(false);
    }
}