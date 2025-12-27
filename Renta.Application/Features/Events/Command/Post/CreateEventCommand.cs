using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Events.Command.Post;

public record CreateEventCommand : ICommand<CreateEventResponse>
{
    public string? Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal GeneralTicketPrice { get; set; }
    public decimal VIPTicketPrice { get; set; }
    public decimal BackstageTicketPrice { get; set; }
    public int TotalCapacity { get; set; }
    public string GeneralBenefits { get; set; } = string.Empty;
    public string VIPBenefits { get; set; } = string.Empty;
    public string BackstageBenefits { get; set; } = string.Empty;
    public IEnumerable<string>? Images { get; set; } = [];
}
