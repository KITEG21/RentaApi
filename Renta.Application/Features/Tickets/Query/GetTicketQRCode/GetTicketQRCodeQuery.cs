using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Tickets.Query.GetTicketQRCode;

public record GetTicketQRCodeQuery : ICommand<byte[]>
{
    public Guid TicketId { get; set; }
}