using FluentValidation;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Cars.Command.Patch;

public class PatchCarStatusCommandValidator : AbstractValidator<PatchCarStatusCommand>
{
    public PatchCarStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Car ID is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Invalid status value. Must be Available, Reserved, or Sold.");
    }
}