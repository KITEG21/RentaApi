using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Vehicles;

namespace Renta.Infrastructure.Persistence.Configurations;

public class YachtConfiguration : IEntityTypeConfiguration<Yacht>
{
    public void Configure(EntityTypeBuilder<Yacht> builder)
    {
        builder.ToTable("Yachts");
        
        builder.HasKey(y => y.Id);
        
        builder.Property(y => y.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(y => y.SizeFt)
            .IsRequired();
        
        builder.Property(y => y.Capacity)
            .IsRequired();
        
        builder.Property(y => y.Description)
            .HasMaxLength(2000);
        
        builder.Property(y => y.IncludedServices)
            .HasMaxLength(2000);
        
        builder.Property(y => y.PricePerHour)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(y => y.PricePerDay)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(y => y.AvailableRoutes)
            .HasMaxLength(2000);
        
        builder.Property(y => y.AvailabilityStatus)
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne(y => y.Owner)
            .WithMany()
            .HasForeignKey(y => y.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Navigation properties
        builder.HasMany(y => y.Photos)
            .WithOne()
            .HasForeignKey(p => p.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(y => y.Videos)
            .WithOne()
            .HasForeignKey(v => v.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(y => y.OwnerId);
        builder.HasIndex(y => y.AvailabilityStatus);
        builder.HasIndex(y => y.Name);
    }
}