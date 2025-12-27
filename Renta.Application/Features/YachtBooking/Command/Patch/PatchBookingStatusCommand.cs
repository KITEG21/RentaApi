using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.YachtBooking.Command.Patch;

public record PatchBookingStatusCommand : ICommand<PatchBookingStatusResponse>
{
    public Guid Id { get; set; }
    public BookingStatus BookingStatus { get; set; }
}
