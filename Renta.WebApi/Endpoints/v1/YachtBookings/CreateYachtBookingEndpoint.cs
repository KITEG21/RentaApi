using FastEndpoints;
using Renta.Application.Features.YachtBooking.Command.Post;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class CreateYachtBookingEndpoint : CoreEndpoint<CreateYachtBookingCommand, CreateYachtBookingResponse>
{
    public override void Configure()
    {
        Post("/yacht-booking");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Create a new yacht booking")
            .WithDescription("Creates a new yacht booking for the authenticated client.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CreateYachtBookingCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
