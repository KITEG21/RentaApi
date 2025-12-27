using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.Yacht.Query.GetAll;

public record GetAllYachtsCommand : ICommand<PagedResponse<GetAllYachtsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;

}
