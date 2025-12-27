using System;
using FastEndpoints;
using Renta.Application.Features.Cars.Command.PatchBaseStatusEntity;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class PatchCarBaseStatusEntityEndpoint : CoreEndpoint<PatchCarBaseStatusEntityCommand, PatchCarBaseStatusEntityResponse>
{
    public override void Configure()
    {
        Patch("/car/{Id}/base-status");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Updates car base status entity")
            .WithDescription("Updates the base status entity of a car (Active, Inactive, InEdition, or Deleted).")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchCarBaseStatusEntityCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}

