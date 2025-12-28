using FastEndpoints;

namespace Renta.Application.Features.YachtCalendar.Query.GetCalendar;

public record GetYachtCalendarQuery : ICommand<GetYachtCalendarResponse>
{
    public Guid YachtId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
