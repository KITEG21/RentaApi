using System;
using FastEndpoints;
using Renta.Application.Features.Events.Command.Patch;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class PatchEventStatusEndpoint : CoreEndpoint<PatchEventStatusCommand, PatchEventStatusResponse>
{
    public override void Configure()
    {
        Patch("/event/{Id}/status");
        Roles("Admin");
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Updates event status")
            .WithDescription("Updates the status of an event (Active, Cancelled, Completed, or Postponed).")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchEventStatusCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
