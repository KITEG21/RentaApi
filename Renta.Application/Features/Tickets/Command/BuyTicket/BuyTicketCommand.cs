using System;
using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Command.BuyTicket;

public record BuyTicketCommand : ICommand<BuyTicketResponse>
{
    public Guid EventId { get; set; }
    public TicketType TicketType { get; set; }

    public string? PaymentMethodToken { get; set; }
    public string? PaymentMethodType { get; set; } = "Stripe"; 

}
