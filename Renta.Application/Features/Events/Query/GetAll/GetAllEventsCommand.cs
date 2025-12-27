using System;
using System.Windows.Input;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.Events.Query.GetAll;

public record GetAllEventsCommand : ICommand<PagedResponse<GetAllEventsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;
}
