using FastEndpoints;
using Renta.Application.Features.YachtBooking.Query.GetAvailability;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class GetYachtAvailabilityEndpoint : CoreEndpoint<GetYachtAvailabilityQuery, GetYachtAvailabilityResponse>
{
    public override void Configure()
    {
        Get("/yacht-booking/availability/{yachtId}/{date}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Get yacht availability")
            .WithDescription("Retrieves the availability of a yacht for a specific date, showing unavailable time slots.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetYachtAvailabilityQuery req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
