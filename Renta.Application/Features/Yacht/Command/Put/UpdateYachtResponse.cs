namespace Renta.Application.Features.Yacht.Command.Put;

public record class UpdateYachtResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int SizeFt { get; init; }
    public int Capacity { get; init; }
    public decimal PricePerHour { get; init; }
    public decimal PricePerDay { get; init; }
}
