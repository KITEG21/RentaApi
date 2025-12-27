using Renta.Domain.Enums;

namespace Renta.Application.Features.Yacht.Command.Patch;

public record class PatchYachtStatusResponse
{
    public Guid Id { get; init; }
    public RentAvailabilityStatus AvailabilityStatus { get; init; }
}
