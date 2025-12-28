using FastEndpoints;
using Renta.Application.Features.Tickets.Command.CreatePaymentIntent;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class CreatePaymentIntentEndpoint : CoreEndpoint<CreatePaymentIntentCommand, CreatePaymentIntentResponse>
{
    public override void Configure()
    {
        Post("/ticket/payment-intent");
        Roles("Client", "Admin");
        Summary(s =>
        {
            s.Summary = "Create Stripe payment intent";
            s.Description = "Creates a payment intent for ticket purchase.";
        });
        base.Configure();
    }

    public override async Task HandleAsync(CreatePaymentIntentCommand req, CancellationToken ct)
    {
        var response = await req.ExecuteAsync(ct);
        await Send.OkAsync(response, ct);
    }
}