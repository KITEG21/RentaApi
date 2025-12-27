using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.YachtBooking.Query.GetAll;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class GetAllYachtBookingsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetAllYachtBookingsResponse>>
{
    public override void Configure()
    {
        Get("/yacht-booking");
        Roles("Client", "Admin", "Dealer");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Get all yacht bookings")
            .WithDescription("Retrieves a paginated list of yacht bookings. Clients see only their bookings, while admins and dealers see all.")
        );
        RequestBinder(new QueryRequestBinder());
        base.Configure();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetAllYachtBookingsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
