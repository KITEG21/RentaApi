using FluentValidation;

namespace Renta.Application.Features.Yacht.Command.Put;

public class UpdateYachtCommandValidator : AbstractValidator<UpdateYachtCommand>
{
    public UpdateYachtCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Yacht ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.SizeFt)
            .GreaterThan(0).WithMessage("Size must be greater than 0 feet.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.IncludedServices)
            .NotEmpty().WithMessage("Included services are required.")
            .MaximumLength(500).WithMessage("Included services must not exceed 500 characters.");

        RuleFor(x => x.PricePerHour)
            .GreaterThan(0).WithMessage("Price per hour must be greater than 0.");

        RuleFor(x => x.PricePerDay)
            .GreaterThan(0).WithMessage("Price per day must be greater than 0.");

        RuleFor(x => x.AvailableRoutes)
            .NotEmpty().WithMessage("Available routes are required.")
            .MaximumLength(500).WithMessage("Available routes must not exceed 500 characters.");

        RuleFor(x => x.Images)
            .Must(images => images == null || images.All(img => !string.IsNullOrWhiteSpace(img)))
            .WithMessage("All image URLs must be valid and not empty.");
    }
}
