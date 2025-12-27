using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Bookings;

namespace Renta.Infrastructure.Persistence.Configurations;

public class YachtBookingConfiguration : IEntityTypeConfiguration<YachtBooking>
{
    public void Configure(EntityTypeBuilder<YachtBooking> builder)
    {
        builder.ToTable("YachtBookings");
        
        builder.HasKey(yb => yb.Id);
        
        builder.Property(yb => yb.Date)
            .IsRequired();
        
        builder.Property(yb => yb.StartTime)
            .IsRequired();
        
        builder.Property(yb => yb.EndTime)
            .IsRequired();
        
        builder.Property(yb => yb.AdditionalServices)
            .HasMaxLength(2000);
        
        builder.Property(yb => yb.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(yb => yb.PaymentStatus)
            .IsRequired();
        
        builder.Property(yb => yb.BookingStatus)
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne(yb => yb.Yacht)
            .WithMany()
            .HasForeignKey(yb => yb.YachtId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(yb => yb.Client)
            .WithMany()
            .HasForeignKey(yb => yb.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(yb => yb.YachtId);
        builder.HasIndex(yb => yb.ClientId);
        builder.HasIndex(yb => yb.Date);
        builder.HasIndex(yb => new { yb.YachtId, yb.Date });
    }
}