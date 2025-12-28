namespace Renta.Application.Features.Tickets.Command.CreatePaymentIntent;

public record CreatePaymentIntentResponse
{
    public string ClientSecret { get; init; } = string.Empty;
    public string PaymentIntentId { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}
