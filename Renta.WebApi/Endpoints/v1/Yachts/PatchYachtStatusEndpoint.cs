using System;
using FastEndpoints;
using Renta.Application.Features.Yacht.Command.Patch;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Yachts;

public class PatchYachtStatusEndpoint : CoreEndpoint<PatchYachtStatusCommand, PatchYachtStatusResponse>
{
    public override void Configure()
    {
        Patch("/yacht/{Id}/status");
        Roles("Admin");
        Description(b => b
            .WithTags(RouteGroup.Yachts)
            .WithSummary("Updates yacht status")
            .WithDescription("Updates the availability status of a yacht (Available, Rented, or UnderMaintenance).")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchYachtStatusCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
