using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Multimedia;

public class Photo : Entity
{
    public string ImageUrl { get; set; } = string.Empty;
    public int VisualizationOrder { get; set; }
    public PhotoType Type { get; set; }
    public Guid? CarId { get; set; }
    public Guid? YachtId { get; set; }
    public Guid? EventId { get; set; }
    
    // Navigation properties
    public Car? Car { get; set; }
    public Yacht? Yacht { get; set; }
    public Event? Event { get; set; }
}
