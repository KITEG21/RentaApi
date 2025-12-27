using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.YachtBooking.Query.GetAll;

public record GetAllYachtBookingsCommand : ICommand<PagedResponse<GetAllYachtBookingsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;
}
