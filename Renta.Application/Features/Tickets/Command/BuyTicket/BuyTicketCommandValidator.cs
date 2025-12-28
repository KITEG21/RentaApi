using FluentValidation;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Tickets.Command.BuyTicket;

public class BuyTicketCommandValidator : AbstractValidator<BuyTicketCommand>
{
    public BuyTicketCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.TicketType)
            .IsInEnum()
            .WithMessage("Invalid ticket type");
    }
}
