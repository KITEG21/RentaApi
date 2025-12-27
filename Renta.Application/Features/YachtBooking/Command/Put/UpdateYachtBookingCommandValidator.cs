using FluentValidation;

namespace Renta.Application.Features.YachtBooking.Command.Put;

public class UpdateYachtBookingCommandValidator : AbstractValidator<UpdateYachtBookingCommand>
{
    public UpdateYachtBookingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Booking ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date cannot be in the past.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0).WithMessage("Total price must be greater than zero.");
    }
}
