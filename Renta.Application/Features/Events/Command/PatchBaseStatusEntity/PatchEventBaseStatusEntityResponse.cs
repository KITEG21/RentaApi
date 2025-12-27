namespace Renta.Application.Features.Events.Command.PatchBaseStatusEntity;

public record PatchEventBaseStatusEntityResponse
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
