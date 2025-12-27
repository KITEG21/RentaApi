using Renta.Domain.Enums;

namespace Renta.Application.Features.Events.Query.GetById;

public record GetEventByIdResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string EventType { get; init; } = string.Empty;
    public DateTime EventDate { get; init; }
    public string Location { get; init; } = string.Empty;
    public decimal GeneralTicketPrice { get; init; }
    public decimal VIPTicketPrice { get; init; }
    public decimal BackstageTicketPrice { get; init; }
    public int TotalCapacity { get; init; }
    public int AvailableTickets { get; init; }
    public string GeneralBenefits { get; init; } = string.Empty;
    public string VIPBenefits { get; init; } = string.Empty;
    public string BackstageBenefits { get; init; } = string.Empty;
    public EventStatus Status { get; init; }
}
