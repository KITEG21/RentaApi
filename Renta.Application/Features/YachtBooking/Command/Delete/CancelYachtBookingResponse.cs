namespace Renta.Application.Features.YachtBooking.Command.Delete;

public record CancelYachtBookingResponse
{
    public Guid Id { get; init; }
    public string BookingStatus { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
