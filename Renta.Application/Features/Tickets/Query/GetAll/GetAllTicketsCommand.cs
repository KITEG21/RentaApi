using System;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.Tickets.Query.GetAll;

public record GetAllTicketsCommand : ICommand<PagedResponse<GetAllTicketsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;
}
