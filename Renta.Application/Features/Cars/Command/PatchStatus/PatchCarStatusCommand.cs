using System.Windows.Input;
using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Cars.Command.Patch;

public record PatchCarStatusCommand : ICommand<PatchCarStatusResponse>
{
    public Guid Id { get; set; }
    public SellStatus Status { get; set; }
}