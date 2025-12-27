using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Cars.Command.Put;

public record UpdateCarCommand : ICommand<UpdateCarResponse>
{
    public Guid Id { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Miles { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MechanicalDetails { get; set; } = string.Empty;
    public string PaymentConditions { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public IEnumerable<string>? Images { get; set; } = [];
}