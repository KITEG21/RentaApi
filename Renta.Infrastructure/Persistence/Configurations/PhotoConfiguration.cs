using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Multimedia;

namespace Renta.Infrastructure.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(p => p.VisualizationOrder)
            .IsRequired();
        
        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion<int>();
        
        // Foreign keys
        builder.Property(p => p.CarId);
        builder.Property(p => p.YachtId);
        builder.Property(p => p.EventId);
        
        // Relationships
        builder.HasOne(p => p.Car)
            .WithMany(c => c.Photos)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Yacht)
            .WithMany(y => y.Photos)
            .HasForeignKey(p => p.YachtId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Event)
            .WithMany(e => e.Photos)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(p => p.CarId);
        builder.HasIndex(p => p.YachtId);
        builder.HasIndex(p => p.EventId);
        builder.HasIndex(p => new { p.CarId, p.VisualizationOrder });
        builder.HasIndex(p => new { p.YachtId, p.VisualizationOrder });
        builder.HasIndex(p => new { p.EventId, p.VisualizationOrder });
    }
}