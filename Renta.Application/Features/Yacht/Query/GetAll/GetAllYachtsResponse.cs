namespace Renta.Application.Features.Yacht.Query.GetAll;

public record GetAllYachtsResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int SizeFt { get; init; }
    public int Capacity { get; init; }
    public decimal PricePerHour { get; init; }
    public decimal PricePerDay { get; init; }
}
