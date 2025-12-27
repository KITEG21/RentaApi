using FastEndpoints;
using Renta.Application.Features.YachtBooking.Query.GetById;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class GetYachtBookingByIdEndpoint : CoreEndpoint<GetYachtBookingByIdCommand, GetYachtBookingByIdResponse>
{
    public override void Configure()
    {
        Get("/yacht-booking/{Id}");
        Roles("Client", "Admin", "Dealer");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Get yacht booking by ID")
            .WithDescription("Retrieves a specific yacht booking by its ID. Clients can only view their own bookings.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetYachtBookingByIdCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
