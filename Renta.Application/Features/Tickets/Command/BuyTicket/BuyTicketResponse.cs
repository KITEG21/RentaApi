using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Command.BuyTicket;

public record BuyTicketResponse
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public string EventTitle { get; init; } = string.Empty;
    public TicketType TicketType { get; init; }
    public decimal PricePaid { get; init; }
    public string QRCode { get; init; } = string.Empty;
    public TicketStatus Status { get; init; }
    public DateTime PurchaseDate { get; init; }
}
