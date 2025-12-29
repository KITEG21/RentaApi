using FastEndpoints;

namespace Renta.Application.Features.Files.Command.Delete;

public record DeleteFileCommand : ICommand<DeleteFileResponse>
{
    public string PublicId { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; } // Optional: delete by PhotoId

}
