using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Yachts.Command.Post;

public record CreateYachtCommand : ICommand<CreateYachtResponse>
{
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