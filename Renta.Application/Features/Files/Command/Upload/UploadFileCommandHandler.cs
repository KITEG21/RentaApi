using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Multimedia;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Files.Command.Upload;

public class UploadFileCommandHandler : CoreCommandHandler<UploadFileCommand, UploadFileResponse>
{
    private readonly IFileService _fileService;

    public UploadFileCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        IFileService fileService) : base(activeUserSession, unitOfWork)
    {
        _fileService = fileService;
    }

    public override async Task<UploadFileResponse> ExecuteAsync(
        UploadFileCommand command,
        CancellationToken ct = default)
    {
        if (command.File == null || command.File.Length == 0)
        {
            ThrowError("No file provided", 400);
        }

        const long maxFileSize = 10 * 1024 * 1024;
        if (command.File.Length > maxFileSize)
        {
            ThrowError("File size exceeds 10MB limit", 400);
        }

        // Validate that exactly one entity is specified
        var entityCount = new[] { command.CarId, command.YachtId, command.EventId }
            .Count(id => id.HasValue && id.Value != Guid.Empty);

        if (entityCount != 1)
        {
            ThrowError("Exactly one entity (CarId, YachtId, or EventId) must be specified", 400);
        }

        // Validate that the entity exists
        await ValidateEntityExists(command, ct);

        var folder = command.Folder?.Trim();
        if (string.IsNullOrWhiteSpace(folder) || string.Equals(folder, "general", StringComparison.OrdinalIgnoreCase))
        {
            if (command.CarId.HasValue && command.CarId.Value != Guid.Empty)
                folder = $"car/{command.CarId.Value}";
            else if (command.YachtId.HasValue && command.YachtId.Value != Guid.Empty)
                folder = $"yacht/{command.YachtId.Value}";
            else if (command.EventId.HasValue && command.EventId.Value != Guid.Empty)
                folder = $"event/{command.EventId.Value}";
            else
                folder = "general";
        }

        var result = await _fileService.UploadAsync(command.File, folder);

        // Create Photo entity and add to the entity's collection
        var photoRepo = UnitOfWork!.WriteDbRepository<Photo>();
        var photo = new Photo
        {
            ImageUrl = result.SecureUrl.ToString(),
            VisualizationOrder = command.VisualizationOrder,
            Type = command.Type
        };

        // Set only the FK that was provided
        if (command.CarId.HasValue && command.CarId.Value != Guid.Empty)
            photo.CarId = command.CarId.Value;
        else if (command.YachtId.HasValue && command.YachtId.Value != Guid.Empty)
            photo.YachtId = command.YachtId.Value;
        else if (command.EventId.HasValue && command.EventId.Value != Guid.Empty)
            photo.EventId = command.EventId.Value;

        await photoRepo.SaveAsync(photo, false);
        await UnitOfWork.CommitChangesAsync();

        return new UploadFileResponse
        {
            PhotoId = photo.Id,
            PublicId = result.PublicId,
            Url = result.Url.ToString(),
            SecureUrl = result.SecureUrl.ToString(),
            Width = result.Width,
            Height = result.Height,
            Format = result.Format,
            Size = result.Bytes
        };
    }

    private async Task ValidateEntityExists(UploadFileCommand command, CancellationToken ct)
    {
        if (command.CarId.HasValue && command.CarId.Value != Guid.Empty)
        {
            var carRepo = UnitOfWork!.ReadDbRepository<Car>();
            var carExists = await carRepo.GetAll()
                .AnyAsync(c => c.Id == command.CarId.Value, ct);

            if (!carExists)
                ThrowError($"Car with ID {command.CarId} not found", 404);
        }
        else if (command.YachtId.HasValue && command.YachtId.Value != Guid.Empty)
        {
            var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
            var yachtExists = await yachtRepo.GetAll()
                .AnyAsync(y => y.Id == command.YachtId.Value, ct);

            if (!yachtExists)
                ThrowError($"Yacht with ID {command.YachtId} not found", 404);
        }
        else if (command.EventId.HasValue && command.EventId.Value != Guid.Empty)
        {
            var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
            var eventExists = await eventRepo.GetAll()
                .AnyAsync(e => e.Id == command.EventId.Value, ct);

            if (!eventExists)
                ThrowError($"Event with ID {command.EventId} not found", 404);
        }
    }
}