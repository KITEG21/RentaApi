using System.Windows.Input;
using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Yacht.Command.Patch;

public record PatchYachtStatusCommand : ICommand<PatchYachtStatusResponse>
{
    public Guid Id { get; set; }
    public RentAvailabilityStatus AvailabilityStatus { get; set; }
}
