using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renta.Domain.Entities.Multimedia;

namespace Renta.Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("Videos");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.VideoUrl)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(v => v.Duration)
            .IsRequired();
        
        builder.Property(v => v.EntityId)
            .IsRequired();
        
        // Indexes
        builder.HasIndex(v => v.EntityId);
    }
}