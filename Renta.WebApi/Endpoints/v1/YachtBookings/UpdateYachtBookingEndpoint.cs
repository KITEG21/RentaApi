using FastEndpoints;
using Renta.Application.Features.YachtBooking.Command.Put;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class UpdateYachtBookingEndpoint : CoreEndpoint<UpdateYachtBookingCommand, UpdateYachtBookingResponse>
{
    public override void Configure()
    {
        Put("/yacht-booking/{Id}");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Update a yacht booking")
            .WithDescription("Updates an existing yacht booking. Only pending bookings can be updated.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(UpdateYachtBookingCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
