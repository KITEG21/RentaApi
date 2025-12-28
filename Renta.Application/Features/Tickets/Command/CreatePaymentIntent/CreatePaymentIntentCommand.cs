using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Command.CreatePaymentIntent;

public record CreatePaymentIntentCommand : ICommand<CreatePaymentIntentResponse>
{
    public Guid EventId { get; set; }
    public TicketType TicketType { get; set; }
}