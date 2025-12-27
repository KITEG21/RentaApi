using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Bookings;

namespace Renta.Infrastructure.Persistence.Configurations;

public class YachtCalendarConfiguration : IEntityTypeConfiguration<YachtCalendar>
{
    public void Configure(EntityTypeBuilder<YachtCalendar> builder)
    {
        builder.ToTable("YachtCalendars");
        
        builder.HasKey(yc => yc.Id);
        
        builder.Property(yc => yc.Date)
            .IsRequired();
        
        builder.Property(yc => yc.StartTime)
            .IsRequired();
        
        builder.Property(yc => yc.EndTime)
            .IsRequired();
        
        builder.Property(yc => yc.Status)
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne(yc => yc.Yacht)
            .WithMany()
            .HasForeignKey(yc => yc.YachtId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(yc => yc.YachtId);
        builder.HasIndex(yc => yc.Date);
        builder.HasIndex(yc => new { yc.YachtId, yc.Date, yc.StartTime });
    }
}