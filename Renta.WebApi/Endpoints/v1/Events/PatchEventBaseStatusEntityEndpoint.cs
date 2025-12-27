using System;
using FastEndpoints;
using Renta.Application.Features.Events.Command.PatchBaseStatusEntity;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class PatchEventBaseStatusEntityEndpoint : CoreEndpoint<PatchEventBaseStatusEntityCommand, PatchEventBaseStatusEntityResponse>
{
    public override void Configure()
    {
        Patch("/event/{Id}/base-status");
        Roles("Admin");
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Updates event base status entity")
            .WithDescription("Updates the base status entity of an event (Active, Inactive, InEdition, or Deleted).")
        );
    }

    public override async Task HandleAsync(PatchEventBaseStatusEntityCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
