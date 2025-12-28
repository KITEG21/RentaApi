using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.YachtCalendar.Command.BulkSetAvailability;

public record BulkSetYachtCalendarCommand : ICommand<BulkSetYachtCalendarResponse>
{
    public Guid YachtId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public CalendarStatus Status { get; set; } = CalendarStatus.Blocked;
    public string Reason { get; set; } = string.Empty;
    public List<DayOfWeek>? DaysOfWeek { get; set; }
}
