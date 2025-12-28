namespace Renta.Application.Features.YachtCalendar.Query.GetCalendar;

public record GetYachtCalendarResponse
{
    public Guid YachtId { get; init; }
    public string YachtName { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public List<CalendarEntryDto> Entries { get; init; } = new();
}

public record CalendarEntryDto
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
}
