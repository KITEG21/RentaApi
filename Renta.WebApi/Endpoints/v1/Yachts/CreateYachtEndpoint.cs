using System;
using FastEndpoints;
using Renta.Application.Features.Yachts.Command.Post;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class CreateYachtEndpoint : CoreEndpoint<CreateYachtCommand, CreateYachtResponse>
{
    public override void Configure()
    {
        Post("/yacht");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Creates a new yacht")
            .WithDescription("Creates a new yacht with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CreateYachtCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}