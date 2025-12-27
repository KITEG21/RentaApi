using FastEndpoints;
using Renta.Application.Features.YachtBooking.Command.Patch;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.YachtBookings;

public class PatchPaymentStatusEndpoint : CoreEndpoint<PatchPaymentStatusCommand, PatchPaymentStatusResponse>
{
    public override void Configure()
    {
        Patch("/yacht-booking/{Id}/payment-status");
        Roles("Admin", "Dealer");
        Description(b => b
            .WithTags(RouteGroup.YachtBookings)
            .WithSummary("Update payment status")
            .WithDescription("Updates the payment status. Only admins and dealers can perform this action.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(PatchPaymentStatusCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
