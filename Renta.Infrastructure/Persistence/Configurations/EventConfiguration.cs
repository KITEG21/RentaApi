using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Events;

namespace Renta.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e => e.Description)
            .HasMaxLength(2000);
        
        builder.Property(e => e.EventType)
            .HasMaxLength(100);
        
        builder.Property(e => e.EventDate)
            .IsRequired();
        
        builder.Property(e => e.Location)
            .HasMaxLength(300);
        
        builder.Property(e => e.GeneralTicketPrice)
            .HasPrecision(18, 2);
        
        builder.Property(e => e.VIPTicketPrice)
            .HasPrecision(18, 2);
        
        builder.Property(e => e.BackstageTicketPrice)
            .HasPrecision(18, 2);
        
        builder.Property(e => e.TotalCapacity)
            .IsRequired();
        
        builder.Property(e => e.AvailableTickets)
            .IsRequired();
        
        builder.Property(e => e.GeneralBenefits)
            .HasMaxLength(1000);
        
        builder.Property(e => e.VIPBenefits)
            .HasMaxLength(1000);
        
        builder.Property(e => e.BackstageBenefits)
            .HasMaxLength(1000);
        
        builder.Property(e => e.Status)
            .IsRequired();
        
        // Navigation properties
        builder.HasMany(e => e.Tickets)
            .WithOne(t => t.Event)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.Photos)
            .WithOne()
            .HasForeignKey(p => p.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.Videos)
            .WithOne()
            .HasForeignKey(v => v.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(e => e.EventDate);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.EventType);
    }
}