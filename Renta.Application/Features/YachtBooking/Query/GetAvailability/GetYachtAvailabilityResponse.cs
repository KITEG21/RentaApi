namespace Renta.Application.Features.YachtBooking.Query.GetAvailability;

public record GetYachtAvailabilityResponse
{
    public Guid YachtId { get; init; }
    public string YachtName { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public List<UnavailableTimeSlot> UnavailableSlots { get; init; } = new();
    public bool IsFullyBooked { get; init; }
}

public record UnavailableTimeSlot
{
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
}
