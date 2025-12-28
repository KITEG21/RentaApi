using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.Tickets.Query.GetAll;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class GetAllTicketsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetAllTicketsResponse>>
{
    public override void Configure()
    {
        Get("/ticket");
        Roles("Admin");
        Description(b => b
            .WithTags(RouteGroup.Tickets)
            .WithSummary("Get all tickets (Admin only)")
            .WithDescription("Retrieves a paginated list of all tickets in the system. Only accessible by administrators.")
        );
        RequestBinder(new QueryRequestBinder());
        base.Configure();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetAllTicketsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
