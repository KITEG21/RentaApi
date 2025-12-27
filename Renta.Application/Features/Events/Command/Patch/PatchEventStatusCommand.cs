using System.Windows.Input;
using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Events.Command.Patch;

public record PatchEventStatusCommand : ICommand<PatchEventStatusResponse>
{
    public Guid Id { get; set; }
    public EventStatus Status { get; set; }
}
