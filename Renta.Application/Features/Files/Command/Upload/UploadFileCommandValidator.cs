using FluentValidation;

namespace Renta.Application.Features.Files.Command.Upload;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required");

        RuleFor(x => x)
            .Must(x => (x.CarId.HasValue && x.CarId.Value != Guid.Empty) ||
                       (x.YachtId.HasValue && x.YachtId.Value != Guid.Empty) ||
                       (x.EventId.HasValue && x.EventId.Value != Guid.Empty))
            .WithMessage("One of CarId, YachtId, or EventId must be provided");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid photo type");

        RuleFor(x => x.VisualizationOrder)
            .GreaterThanOrEqualTo(0);
    }
}