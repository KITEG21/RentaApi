using System;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.Events.Query.GetAll;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class GetAllEventsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetAllEventsResponse>>
{
    public override void Configure()
    {
        Get("/event");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Retrieves all events")
            .WithDescription("Retrieves a paginated list of all events with optional filtering and sorting.")
        );
        base.Configure();
        RequestBinder(new QueryRequestBinder());    
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetAllEventsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
