namespace Renta.Application.Features.YachtCalendar.Command.Delete;

public record DeleteYachtCalendarResponse
{
    public Guid Id { get; init; }
    public string Message { get; init; } = string.Empty;
}
