using System;
using FastEndpoints;
using Renta.Application.Features.Yacht.Query.GetById;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class GetYachtByIdEndpoint : CoreEndpoint<GetYachtByIdCommand, GetYachtByIdResponse>
{
    public override void Configure()
    {
        Get("/yacht/{Id}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Retrieves a yacht by ID")
            .WithDescription("Retrieves detailed information about a specific yacht by its unique identifier.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetYachtByIdCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
