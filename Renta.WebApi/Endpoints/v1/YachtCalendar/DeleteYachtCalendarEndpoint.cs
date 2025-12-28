using FastEndpoints;
using Renta.Application.Features.YachtCalendar.Command.Delete;
using Renta.WebApi.Authorization;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtCalendar;

public class DeleteYachtCalendarEndpoint : CoreEndpoint<DeleteYachtCalendarCommand, DeleteYachtCalendarResponse>
{
    public override void Configure()
    {
        Delete("/yacht-calendar/{Id}");
        Policies(AuthorizationPolicies.DealerOrAdmin);
        Description(b => b
            .WithTags(RouteGroup.YachtCalendar)
            .WithSummary("Delete yacht calendar entry")
            .WithDescription("Remove a blocked time slot from yacht calendar")
        );
        base.Configure();
    }

    public override async Task HandleAsync(DeleteYachtCalendarCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
