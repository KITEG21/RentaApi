using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.Tickets.Query.GetMyTickets;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class GetMyTicketsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetMyTicketsResponse>>
{
    public override void Configure()
    {
        Get("/mis-tickets");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.Tickets)
            .WithSummary("Get current user's tickets")
            .WithDescription("Retrieves a paginated list of tickets purchased by the currently authenticated user.")
        );
        RequestBinder(new QueryRequestBinder());
        base.Configure();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetMyTicketsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
