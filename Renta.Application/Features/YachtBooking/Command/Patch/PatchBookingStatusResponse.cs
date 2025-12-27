namespace Renta.Application.Features.YachtBooking.Command.Patch;

public record PatchBookingStatusResponse
{
    public Guid Id { get; init; }
    public string BookingStatus { get; init; } = string.Empty;
}
