namespace Renta.Application.Features.Cars.Query.GetAll;

public record GetAllCarsResponse
{
    public Guid Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }  
    public decimal Price { get; set; }
}
