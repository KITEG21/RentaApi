namespace Renta.Application.Features.YachtBooking.Command.Put;

public record UpdateYachtBookingResponse
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public decimal TotalPrice { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string BookingStatus { get; init; } = string.Empty;
}
