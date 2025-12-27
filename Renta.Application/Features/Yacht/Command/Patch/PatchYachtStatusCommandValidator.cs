using FluentValidation;

namespace Renta.Application.Features.Yacht.Command.Patch;

public class PatchYachtStatusCommandValidator : AbstractValidator<PatchYachtStatusCommand>
{
    public PatchYachtStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Yacht ID is required.");

        RuleFor(x => x.AvailabilityStatus)
            .IsInEnum().WithMessage("Invalid status value. Must be Available, Rented, or UnderMaintenance.");
    }
}
