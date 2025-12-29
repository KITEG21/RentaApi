namespace Renta.Application.Features.Files.Command.Upload;

public record UploadFileResponse
{
    public Guid PhotoId { get; init; }
    public string PublicId { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string SecureUrl { get; init; } = string.Empty;
    public int Width { get; init; }
    public int Height { get; init; }
    public string Format { get; init; } = string.Empty;
    public long Size { get; init; }
}