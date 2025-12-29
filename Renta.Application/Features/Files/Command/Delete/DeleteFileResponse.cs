using System;

namespace Renta.Application.Features.Files.Command.Delete;

public record DeleteFileResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
