using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Events;

namespace Renta.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.TicketType)
            .IsRequired();
        
        builder.Property(t => t.PricePaid)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(t => t.QRCode)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(t => t.Status)
            .IsRequired();
        
        builder.Property(t => t.PurchaseDate)
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne(t => t.Event)
            .WithMany(e => e.Tickets)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t => t.Client)
            .WithMany()
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(t => t.EventId);
        builder.HasIndex(t => t.ClientId);
        builder.HasIndex(t => t.QRCode).IsUnique();
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.PurchaseDate);
    }
}