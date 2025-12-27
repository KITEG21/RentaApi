using Renta.Domain.Enums;

namespace Renta.Application.Features.Cars.Query.GetById;

public record GetCarByIdResponse
{
    public Guid Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal Price { get; init; }
    public int Miles { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string MechanicalDetails { get; init; } = string.Empty;
    public string PaymentConditions { get; init; } = string.Empty;
    public string History { get; init; } = string.Empty;
    public SellStatus Status { get; init; }
    public Guid DealerId { get; init; }
}