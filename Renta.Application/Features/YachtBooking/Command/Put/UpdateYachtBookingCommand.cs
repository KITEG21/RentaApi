using FastEndpoints;

namespace Renta.Application.Features.YachtBooking.Command.Put;

public record UpdateYachtBookingCommand : ICommand<UpdateYachtBookingResponse>
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string AdditionalServices { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
}
