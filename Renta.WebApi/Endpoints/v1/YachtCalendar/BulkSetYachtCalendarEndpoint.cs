using FastEndpoints;
using Renta.Application.Features.YachtCalendar.Command.BulkSetAvailability;
using Renta.WebApi.Authorization;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtCalendar;

public class BulkSetYachtCalendarEndpoint : CoreEndpoint<BulkSetYachtCalendarCommand, BulkSetYachtCalendarResponse>
{
    public override void Configure()
    {
        Post("/yacht-calendar/bulk");
        Policies(AuthorizationPolicies.AdminOnly);
        Description(b => b
            .WithTags(RouteGroup.YachtCalendar)
            .WithSummary("Bulk set yacht calendar availability")
            .WithDescription("Block multiple date/time slots for a yacht at once")
        );
        base.Configure();
    }

    public override async Task HandleAsync(BulkSetYachtCalendarCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
