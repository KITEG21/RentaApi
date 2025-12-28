using System;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.Tickets.Query.GetMyTickets;

public record GetMyTicketsCommand : ICommand<PagedResponse<GetMyTicketsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;
}
