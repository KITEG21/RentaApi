using Renta.Domain.Enums;

namespace Renta.Application.Features.Events.Command.Patch;

public record class PatchEventStatusResponse
{
    public Guid Id { get; init; }
    public EventStatus Status { get; init; }
}
