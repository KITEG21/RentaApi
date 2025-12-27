using System;
using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Events.Command.PatchBaseStatusEntity;

public record PatchEventBaseStatusEntityCommand : ICommand<PatchEventBaseStatusEntityResponse>
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
