using FluentValidation;

namespace Renta.Application.Features.YachtCalendar.Command.BulkSetAvailability;

public class BulkSetYachtCalendarCommandValidator : AbstractValidator<BulkSetYachtCalendarCommand>
{
    public BulkSetYachtCalendarCommandValidator()
    {
        RuleFor(x => x.YachtId)
            .NotEmpty().WithMessage("Yacht ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid calendar status.");
    }
}
