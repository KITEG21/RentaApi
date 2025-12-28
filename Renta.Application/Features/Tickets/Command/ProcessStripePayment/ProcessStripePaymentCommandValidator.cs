using FluentValidation;

namespace Renta.Application.Features.Tickets.Command.ProcessStripePayment;

public class ProcessStripePaymentCommandValidator : AbstractValidator<ProcessStripePaymentCommand>
{
    public ProcessStripePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentIntentId)
            .NotEmpty()
            .WithMessage("Payment Intent ID is required");

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required");

        RuleFor(x => x.AmountInCents)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.TicketType)
            .IsInEnum()
            .WithMessage("Invalid ticket type");
    }
}