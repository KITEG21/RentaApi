using FastEndpoints;
using Renta.Application.Features.YachtCalendar.Query.GetCalendar;
using Renta.WebApi.Binders;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtCalendar;

public class GetYachtCalendarEndpoint : CoreEndpoint<GetYachtCalendarQuery, GetYachtCalendarResponse>
{
    public override void Configure()
    {
        Get("/yacht-calendar/{yachtId}/{startDate}/{endDate}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.YachtCalendar)
            .WithSummary("Get yacht calendar")
            .WithDescription("Retrieve blocked time slots for a yacht within a date range")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetYachtCalendarQuery req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
