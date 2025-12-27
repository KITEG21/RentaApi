using FastEndpoints;

namespace Renta.Application.Features.YachtBooking.Query.GetAvailability;

public record GetYachtAvailabilityQuery : ICommand<GetYachtAvailabilityResponse>
{
    public Guid YachtId { get; set; }
    public DateTime Date { get; set; }
}
