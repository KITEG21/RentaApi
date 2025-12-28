using FastEndpoints;

namespace Renta.Application.Features.YachtCalendar.Command.Delete;

public record DeleteYachtCalendarCommand : ICommand<DeleteYachtCalendarResponse>
{
    public Guid Id { get; set; }
}
