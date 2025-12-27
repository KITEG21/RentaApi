using FastEndpoints;

namespace Renta.Application.Features.YachtBooking.Command.Delete;

public record CancelYachtBookingCommand : ICommand<CancelYachtBookingResponse>
{
    public Guid Id { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
}
