namespace Renta.Application.Features.YachtBooking.Command.Post;

public record CreateYachtBookingResponse
{
    public Guid Id { get; init; }
    public Guid YachtId { get; init; }
    public Guid ClientId { get; init; }
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public decimal TotalPrice { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string BookingStatus { get; init; } = string.Empty;
}
