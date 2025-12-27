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
            .IsRequired();
        
        builder.Property(p => p.EntityId)
            .IsRequired();
        
        // Indexes
        builder.HasIndex(p => p.EntityId);
        builder.HasIndex(p => new { p.EntityId, p.VisualizationOrder });
    }
}