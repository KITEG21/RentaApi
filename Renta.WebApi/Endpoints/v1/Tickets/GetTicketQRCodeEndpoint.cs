using FastEndpoints;
using Renta.Application.Features.Tickets.Query.GetTicketQRCode;

namespace Renta.WebApi.Endpoints.v1.Tickets;

public class GetTicketQRCodeEndpoint : Endpoint<GetTicketQRCodeQuery, byte[]>
{
    public override void Configure()
    {
        Get("/ticket/{TicketId}/qr");
        Roles("Client", "Admin");
        Summary(s =>
        {
            s.Summary = "Get ticket QR code image";
            s.Description = "Returns a PNG image of the ticket's QR code";
        });
    }

    public override async Task HandleAsync(GetTicketQRCodeQuery req, CancellationToken ct)
    {
        var qrImage = await req.ExecuteAsync(ct);
        await Send.OkAsync(qrImage, ct);
    }
}