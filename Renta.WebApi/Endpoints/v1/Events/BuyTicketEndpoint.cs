using System;
using FastEndpoints;
using Renta.Application.Features.Tickets.Command.BuyTicket;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Events;

public class BuyTicketEndpoint : CoreEndpoint<BuyTicketRequest, BuyTicketResponse>
{
    public override void Configure()
    {
        Post("/event/{eventId}/get-ticket");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.Events)
            .WithSummary("Buy event ticket")
            .WithDescription("Purchase a ticket for an event. The ticket price will be determined based on the ticket type selected. Payment processing is not yet implemented.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(BuyTicketRequest req, CancellationToken ct)
    {
        var command = new BuyTicketCommand
        {
            EventId = req.EventId,
            TicketType = req.TicketType
        };

        var result = await command.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}

// Request wrapper to separate route parameter from body
public record BuyTicketRequest
{
    public Guid EventId { get; set; }
    public Domain.Enums.TicketType TicketType { get; set; }
}
