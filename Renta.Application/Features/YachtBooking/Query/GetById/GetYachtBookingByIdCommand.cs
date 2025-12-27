using FastEndpoints;

namespace Renta.Application.Features.YachtBooking.Query.GetById;

public record GetYachtBookingByIdCommand : ICommand<GetYachtBookingByIdResponse>
{
    public Guid Id { get; set; }
}
