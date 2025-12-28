using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Events;

public class Ticket: Entity
{
    public Guid EventId { get; set; }
    public Guid ClientId { get; set; }
    public TicketType TicketType { get; set; }
    public decimal PricePaid { get; set; }
    public string QRCode { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Valid;
    public DateTime PurchaseDate { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? PaymentTransactionId { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? PaymentDate { get; set; }


    // Navigation properties
    public Event Event { get; set; } = null!;
    public User Client { get; set; } = null!;
}
