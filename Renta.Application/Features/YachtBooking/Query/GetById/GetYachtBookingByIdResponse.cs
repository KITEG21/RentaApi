namespace Renta.Application.Features.YachtBooking.Query.GetById;

public record GetYachtBookingByIdResponse
{
    public Guid Id { get; init; }
    public Guid YachtId { get; init; }
    public string YachtName { get; init; } = string.Empty;
    public int YachtSizeFt { get; init; }
    public int YachtCapacity { get; init; }
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public string ClientEmail { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string AdditionalServices { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string BookingStatus { get; init; } = string.Empty;
    public DateTime Created { get; init; }
    public DateTime? LastModified { get; init; }
}
