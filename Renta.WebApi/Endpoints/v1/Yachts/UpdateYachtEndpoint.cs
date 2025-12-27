using System;
using FastEndpoints;
using Renta.Application.Features.Yacht.Command.Put;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class UpdateYachtEndpoint : CoreEndpoint<UpdateYachtCommand, UpdateYachtResponse>
{
    public override void Configure()
    {
        Put("/yacht/{Id}");
        Roles("Admin");
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Updates an existing yacht")
            .WithDescription("Updates an existing yacht with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(UpdateYachtCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
