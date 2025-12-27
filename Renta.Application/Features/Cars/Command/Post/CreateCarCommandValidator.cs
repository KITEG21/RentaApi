using FluentValidation;

namespace Renta.Application.Features.Cars.Command.Post;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50).WithMessage("Brand must not exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.Now.Year).WithMessage($"Year must be between 1900 and {DateTime.Now.Year}.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Miles)
            .GreaterThanOrEqualTo(0).WithMessage("Miles must be greater than or equal to 0.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.MechanicalDetails)
            .NotEmpty().WithMessage("Mechanical details are required.")
            .MaximumLength(500).WithMessage("Mechanical details must not exceed 500 characters.");

        RuleFor(x => x.PaymentConditions)
            .NotEmpty().WithMessage("Payment conditions are required.")
            .MaximumLength(500).WithMessage("Payment conditions must not exceed 500 characters.");

    }
}