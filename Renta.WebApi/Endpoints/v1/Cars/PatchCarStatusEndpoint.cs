using System;
using FastEndpoints;
using Renta.Application.Features.Cars.Command.Patch;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class PatchCarStatusEndpoint : CoreEndpoint<PatchCarStatusCommand, PatchCarStatusResponse>
{
    public override void Configure()
    {
        Patch("/car/{Id}/status");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Updates car status")
            .WithDescription("Updates the status of a car (Available, Reserved, or Sold).")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchCarStatusCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}