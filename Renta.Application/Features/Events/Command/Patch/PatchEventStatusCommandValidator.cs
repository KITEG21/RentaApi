using FluentValidation;

namespace Renta.Application.Features.Events.Command.Patch;

public class PatchEventStatusCommandValidator : AbstractValidator<PatchEventStatusCommand>
{
    public PatchEventStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value. Must be Active, Cancelled, Completed, or Postponed.");
    }
}
