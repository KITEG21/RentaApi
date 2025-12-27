using System;
using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Cars.Command.PatchBaseStatusEntity;

public record PatchCarBaseStatusEntityCommand : ICommand<PatchCarBaseStatusEntityResponse>
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
