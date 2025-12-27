namespace Renta.Application.Features.Yacht.Command.PatchBaseStatusEntity;

public record PatchYachtBaseStatusEntityResponse
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
