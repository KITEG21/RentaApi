using FluentValidation;

namespace Renta.Application.Features.Events.Command.Put;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.EventType)
            .NotEmpty().WithMessage("Event type is required.")
            .MaximumLength(50).WithMessage("Event type must not exceed 50 characters.");

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.Now).WithMessage("Event date must be in the future.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.GeneralTicketPrice)
            .GreaterThanOrEqualTo(0).WithMessage("General ticket price must be greater than or equal to 0.");

        RuleFor(x => x.VIPTicketPrice)
            .GreaterThanOrEqualTo(0).WithMessage("VIP ticket price must be greater than or equal to 0.");

        RuleFor(x => x.BackstageTicketPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Backstage ticket price must be greater than or equal to 0.");

        RuleFor(x => x.TotalCapacity)
            .GreaterThan(0).WithMessage("Total capacity must be greater than 0.");

        RuleFor(x => x.GeneralBenefits)
            .MaximumLength(500).WithMessage("General benefits must not exceed 500 characters.");

        RuleFor(x => x.VIPBenefits)
            .MaximumLength(500).WithMessage("VIP benefits must not exceed 500 characters.");

        RuleFor(x => x.BackstageBenefits)
            .MaximumLength(500).WithMessage("Backstage benefits must not exceed 500 characters.");

        RuleFor(x => x.Images)
            .Must(images => images == null || images.All(img => !string.IsNullOrWhiteSpace(img)))
            .WithMessage("All image URLs must be valid and not empty.");
    }
}
