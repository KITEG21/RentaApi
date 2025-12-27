using System;
using FastEndpoints;
using Renta.Application.Features.Events.Command.Put;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class UpdateEventEndpoint : CoreEndpoint<UpdateEventCommand, UpdateEventResponse>
{
    public override void Configure()
    {
        Put("/event/{Id}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Updates an existing event")
            .WithDescription("Updates an existing event with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(UpdateEventCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
