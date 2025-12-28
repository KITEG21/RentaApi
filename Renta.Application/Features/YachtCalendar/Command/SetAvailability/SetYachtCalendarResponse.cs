namespace Renta.Application.Features.YachtCalendar.Command.SetAvailability;

public record SetYachtCalendarResponse
{
    public Guid Id { get; init; }
    public Guid YachtId { get; init; }
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
