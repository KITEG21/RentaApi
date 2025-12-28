using FastEndpoints;
using Renta.Application.Features.YachtCalendar.Command.SetAvailability;
using Renta.WebApi.Authorization;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtCalendar;

public class SetYachtCalendarEndpoint : CoreEndpoint<SetYachtCalendarCommand, SetYachtCalendarResponse>
{
    public override void Configure()
    {
        Post("/yacht-calendar");
        Policies(AuthorizationPolicies.DealerOrAdmin);
        Description(b => b
            .WithTags(RouteGroup.YachtCalendar)
            .WithSummary("Set yacht calendar availability")
            .WithDescription("Block a time slot for a yacht (maintenance, personal use, etc.)")
        );
        base.Configure();
    }

    public override async Task HandleAsync(SetYachtCalendarCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
