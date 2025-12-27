namespace Renta.Application.Features.Cars.Command.Put;

public record class UpdateCarResponse
{
    public Guid Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal Price { get; init; }
}