using System;
using FastEndpoints;
using Renta.Application.Features.Tickets.Query.GetById;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class GetTicketByIdEndpoint : CoreEndpoint<GetTicketByIdCommand, GetTicketByIdResponse>
{
    public override void Configure()
    {
        Get("/ticket/{Id}");
        Roles("Client", "Admin");
        Description(b => b
            .WithTags(RouteGroup.Tickets)
            .WithSummary("Get ticket by ID")
            .WithDescription("Retrieves detailed information about a specific ticket by its unique identifier.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetTicketByIdCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
