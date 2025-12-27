namespace Renta.Application.Features.YachtBooking.Query.GetAll;

public record GetAllYachtBookingsResponse
{
    public Guid Id { get; init; }
    public Guid YachtId { get; init; }
    public string YachtName { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public decimal TotalPrice { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string BookingStatus { get; init; } = string.Empty;
    public DateTime Created { get; init; }
}
