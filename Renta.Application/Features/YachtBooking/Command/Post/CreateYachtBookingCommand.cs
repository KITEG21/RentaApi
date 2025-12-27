using FastEndpoints;

namespace Renta.Application.Features.YachtBooking.Command.Post;

public record CreateYachtBookingCommand : ICommand<CreateYachtBookingResponse>
{
    public Guid YachtId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string AdditionalServices { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
}
