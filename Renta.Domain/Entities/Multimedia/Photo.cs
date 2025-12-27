using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Multimedia;

public class Photo : Entity
{
    public string ImageUrl { get; set; } = string.Empty;
    public int VisualizationOrder { get; set; }
    public PhotoType Type { get; set; }
    public Guid EntityId { get; set; }
}
