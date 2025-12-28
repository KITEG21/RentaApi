using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Command.ProcessStripePayment;

public record ProcessStripePaymentCommand : ICommand<ProcessStripePaymentResponse>
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public long AmountInCents { get; set; }
    public Guid EventId { get; set; }
    public Guid ClientId { get; set; }
    public TicketType TicketType { get; set; }
}