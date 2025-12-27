using Renta.Domain.Enums;

namespace Renta.Application.Features.Events.Query.GetAll;

public record GetAllEventsResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string EventType { get; init; } = string.Empty;
    public DateTime EventDate { get; init; }
    public string Location { get; init; } = string.Empty;
    public int TotalCapacity { get; init; }
    public int AvailableTickets { get; init; }
    public EventStatus Status { get; init; }
}
