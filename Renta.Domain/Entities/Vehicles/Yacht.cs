using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Multimedia;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Vehicles;

public class Yacht : Entity
{
    public string Name { get; set; } = string.Empty;
    public int SizeFt { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public string IncludedServices { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public decimal PricePerDay { get; set; }
    public string AvailableRoutes { get; set; } = string.Empty;
    public RentAvailabilityStatus AvailabilityStatus { get; set; } = RentAvailabilityStatus.Available;
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Video> Videos { get; set; } = new List<Video>();


}
