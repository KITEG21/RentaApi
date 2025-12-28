using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Query.GetMyTickets;

public record GetMyTicketsResponse
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public string EventTitle { get; init; } = string.Empty;
    public DateTime EventDate { get; init; }
    public string EventLocation { get; init; } = string.Empty;
    public TicketType TicketType { get; init; }
    public decimal PricePaid { get; init; }
    public string QRCode { get; init; } = string.Empty;
    public TicketStatus Status { get; init; }
    public DateTime PurchaseDate { get; init; }
}
