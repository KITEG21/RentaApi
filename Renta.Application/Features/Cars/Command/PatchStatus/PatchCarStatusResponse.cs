using Renta.Domain.Enums;

namespace Renta.Application.Features.Cars.Command.Patch;

public record class PatchCarStatusResponse
{
    public Guid Id { get; init; }
    public SellStatus Status { get; init; }
}