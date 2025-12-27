using System;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;
using Renta.Application.Features.Cars.Query.GetAll;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class GetAllCarsEndpoint : CoreEndpoint<QueryRequest, PagedResponse<GetAllCarsResponse>>
{
    public override void Configure()
    {
        Get("/car");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Retrieves all cars")
            .WithDescription("Retrieves a paginated list of all cars with optional filtering and sorting.")
        );
        RequestBinder(new QueryRequestBinder());
        base.Configure();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var command = new GetAllCarsCommand
        {
            queryRequest = req
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
