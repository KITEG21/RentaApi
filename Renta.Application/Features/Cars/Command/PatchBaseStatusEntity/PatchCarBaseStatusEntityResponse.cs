namespace Renta.Application.Features.Cars.Command.PatchBaseStatusEntity;

public record PatchCarBaseStatusEntityResponse
{
    public Guid Id { get; init; }
    public string BaseStatus { get; init; } = string.Empty;
}
