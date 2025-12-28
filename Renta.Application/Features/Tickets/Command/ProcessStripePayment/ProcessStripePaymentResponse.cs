namespace Renta.Application.Features.Tickets.Command.ProcessStripePayment;

public record class ProcessStripePaymentResponse
{
    public bool Success { get; init; }
    public Guid? TicketId { get; init; }
    public string? ErrorMessage { get; init; }
    public string? QRCode { get; init; }
}
