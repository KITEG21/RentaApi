using FastEndpoints;
using Renta.Application.Features.YachtBooking.Command.Patch;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class PatchBookingStatusEndpoint : CoreEndpoint<PatchBookingStatusCommand, PatchBookingStatusResponse>
{
    public override void Configure()
    {
        Patch("/yacht-booking/{Id}/booking-status");
        Roles("Admin", "Dealer");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Update booking status")
            .WithDescription("Updates the booking status. Only admins and dealers can perform this action.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchBookingStatusCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
