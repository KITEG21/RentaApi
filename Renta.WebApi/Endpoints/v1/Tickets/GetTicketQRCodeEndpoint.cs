using FastEndpoints;
using Renta.Application.Features.Tickets.Query.GetTicketQRCode;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class GetTicketQRCodeEndpoint : Endpoint<GetTicketQRCodeQuery, byte[]>
{
    public override void Configure()
    {
        Get("/ticket/{ticketId}/qr");
        Roles("Client", "Admin");
        Description(b => b
        .WithTags(RouteGroup.Tickets)
        .WithSummary("Get ticket QR code image")
        .WithDescription("Returns a PNG image of the ticket's QR code")
        );
    }

    public override async Task HandleAsync(GetTicketQRCodeQuery req, CancellationToken ct)
    {
        var qrImage = await req.ExecuteAsync(ct);
        await Send.OkAsync(qrImage, ct);
    }
}