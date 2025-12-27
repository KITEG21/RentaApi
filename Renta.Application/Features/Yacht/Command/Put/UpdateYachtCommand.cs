using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Yacht.Command.Put;

public record UpdateYachtCommand : ICommand<UpdateYachtResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int SizeFt { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public string IncludedServices { get; set; } = string.Empty;
    public decimal PricePerHour { get; set; }
    public decimal PricePerDay { get; set; }
    public string AvailableRoutes { get; set; } = string.Empty;
    public IEnumerable<string>? Images { get; set; } = [];
}
