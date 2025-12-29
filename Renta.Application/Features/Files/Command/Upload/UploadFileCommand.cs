using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Files.Command.Upload;

public record UploadFileCommand : ICommand<UploadFileResponse>
{
    public IFormFile File { get; set; } = null!;
    public string Folder { get; set; } = "general";
    
    // Photo entity fields
    public Guid? CarId { get; set; }
    public Guid? YachtId { get; set; }
    public Guid? EventId { get; set; }
    public PhotoType Type { get; set; } = PhotoType.Gallery;
    public int VisualizationOrder { get; set; } = 0;
}