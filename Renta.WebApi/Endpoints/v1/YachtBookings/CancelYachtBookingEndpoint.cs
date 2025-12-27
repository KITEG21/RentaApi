using FastEndpoints;
using Renta.Application.Features.YachtBooking.Command.Delete;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class CancelYachtBookingEndpoint : CoreEndpoint<CancelYachtBookingCommand, CancelYachtBookingResponse>
{
    public override void Configure()
    {
        Delete("/yacht-booking/{Id}");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Cancel a yacht booking")
            .WithDescription("Cancels an existing yacht booking and releases the calendar slot.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CancelYachtBookingCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
