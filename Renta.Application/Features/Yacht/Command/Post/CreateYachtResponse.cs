namespace Renta.Application.Features.Yachts.Command.Post;

public record class CreateYachtResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int SizeFt { get; init; }
    public int Capacity { get; init; }
    public decimal PricePerHour { get; init; }
    public decimal PricePerDay { get; init; }
}