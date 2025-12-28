using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.YachtCalendar.Command.SetAvailability;

public record SetYachtCalendarCommand : ICommand<SetYachtCalendarResponse>
{
    public Guid YachtId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public CalendarStatus Status { get; set; } = CalendarStatus.Blocked;
    public string Reason { get; set; } = string.Empty;
}
