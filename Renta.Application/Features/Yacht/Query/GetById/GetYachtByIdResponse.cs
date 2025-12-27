using Renta.Domain.Enums;

namespace Renta.Application.Features.Yacht.Query.GetById;

public record GetYachtByIdResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int SizeFt { get; init; }
    public int Capacity { get; init; }
    public string Description { get; init; } = string.Empty;
    public string IncludedServices { get; init; } = string.Empty;
    public decimal PricePerHour { get; init; }
    public decimal PricePerDay { get; init; }
    public string AvailableRoutes { get; init; } = string.Empty;
    public RentAvailabilityStatus AvailabilityStatus { get; init; }
    public Guid OwnerId { get; init; }
}
