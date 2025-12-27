namespace Renta.Application.Features.YachtBooking.Command.Patch;

public record PatchPaymentStatusResponse
{
    public Guid Id { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
}
