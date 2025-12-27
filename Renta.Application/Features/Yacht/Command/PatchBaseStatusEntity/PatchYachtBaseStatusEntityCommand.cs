using System;
using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Yacht.Command.PatchBaseStatusEntity;

public record PatchYachtBaseStatusEntityCommand : ICommand<PatchYachtBaseStatusEntityResponse>
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
