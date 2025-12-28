namespace Renta.Application.Features.YachtCalendar.Command.BulkSetAvailability;

public record BulkSetYachtCalendarResponse
{
    public Guid YachtId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int CreatedCount { get; init; }
    public int UpdatedCount { get; init; }
    public int SkippedCount { get; init; }
    public List<DateTime> SkippedDates { get; init; } = new();
    public string Message { get; init; } = string.Empty;
}
