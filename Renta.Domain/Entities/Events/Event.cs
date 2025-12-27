using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Multimedia;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Events;

public class Event: Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    
    // Ticket pricing
    public decimal GeneralTicketPrice { get; set; }
    public decimal VIPTicketPrice { get; set; }
    public decimal BackstageTicketPrice { get; set; }
    
    // Capacity management
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }
    
    // Benefits per ticket type (can be JSON or separate text fields)
    public string GeneralBenefits { get; set; } = string.Empty;
    public string VIPBenefits { get; set; } = string.Empty;
    public string BackstageBenefits { get; set; } = string.Empty;
    
    public EventStatus Status { get; set; } = EventStatus.Active;
    
    // Navigation properties
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Video> Videos { get; set; } = new List<Video>();
}