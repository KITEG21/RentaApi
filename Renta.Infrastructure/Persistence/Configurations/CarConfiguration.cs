using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Vehicles;

namespace Renta.Infrastructure.Persistence.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Brand)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Model)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Year)
            .IsRequired();
        
        builder.Property(c => c.Price)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(c => c.Miles)
            .IsRequired();
        
        builder.Property(c => c.Location)
            .HasMaxLength(200);
        
        builder.Property(c => c.Description)
            .HasMaxLength(2000);
        
        builder.Property(c => c.MechanicalDetails)
            .HasMaxLength(2000);
        
        builder.Property(c => c.PaymentConditions)
            .HasMaxLength(1000);
        
        builder.Property(c => c.History)
            .HasMaxLength(2000);
        
        builder.Property(c => c.Status)
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne(c => c.Dealer)
            .WithMany()
            .HasForeignKey(c => c.DealerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Navigation properties
        // builder.HasMany(c => c.Photos)
        //     .WithOne()
        //     .HasForeignKey(p => p.EntityId)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        // builder.HasMany(c => c.Videos)
        //     .WithOne()
        //     .HasForeignKey(v => v.EntityId)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(c => c.DealerId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Brand);
    }
}