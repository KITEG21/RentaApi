using System;
using FastEndpoints;
using Renta.Application.Features.Yacht.Command.PatchBaseStatusEntity;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class PatchYachtBaseStatusEntityEndpoint : CoreEndpoint<PatchYachtBaseStatusEntityCommand, PatchYachtBaseStatusEntityResponse>
{
    public override void Configure()
    {
        Patch("/yacht/{Id}/base-status");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Updates yacht base status entity")
            .WithDescription("Updates the base status entity of a yacht (Active, Inactive, InEdition, or Deleted).")
        );
    }

    public override async Task HandleAsync(PatchYachtBaseStatusEntityCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
