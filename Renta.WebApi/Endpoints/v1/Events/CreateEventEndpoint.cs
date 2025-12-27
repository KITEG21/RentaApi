using System;
using FastEndpoints;
using Renta.Application.Features.Events.Command.Post;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class CreateEventEndpoint : CoreEndpoint<CreateEventCommand, CreateEventResponse>
{
    public override void Configure()
    {
        Post("/event");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Creates a new event")
            .WithDescription("Creates a new event with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CreateEventCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
