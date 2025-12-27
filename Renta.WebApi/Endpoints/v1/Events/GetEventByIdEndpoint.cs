using System;
using FastEndpoints;
using Renta.Application.Features.Events.Query.GetById;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class GetEventByIdEndpoint : CoreEndpoint<GetEventByIdCommand, GetEventByIdResponse>
{
    public override void Configure()
    {
        Get("/event/{Id}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Retrieves an event by ID")
            .WithDescription("Retrieves detailed information about a specific event by its unique identifier.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetEventByIdCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
