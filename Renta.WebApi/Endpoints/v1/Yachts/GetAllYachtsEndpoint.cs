using System;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.Yacht.Query.GetAll;
using Renta.Application.Features.Yachts.Query.GetAll;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class GetAllYachtsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetAllYachtsResponse>>
{
    public override void Configure()
    {
        Get("/yacht");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Retrieves all yachts")
            .WithDescription("Retrieves a paginated list of all yachts with optional filtering and sorting.")
        );
        RequestBinder(new QueryRequestBinder());
        base.Configure();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetAllYachtsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}