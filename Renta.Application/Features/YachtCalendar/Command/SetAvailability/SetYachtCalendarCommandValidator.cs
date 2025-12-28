using FluentValidation;

namespace Renta.Application.Features.YachtCalendar.Command.SetAvailability;

public class SetYachtCalendarCommandValidator : AbstractValidator<SetYachtCalendarCommand>
{
    public SetYachtCalendarCommandValidator()
    {
        RuleFor(x => x.YachtId)
            .NotEmpty().WithMessage("Yacht ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date cannot be in the past.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid calendar status.");
    }
}
