using System;
using System.Windows.Input;
using FastEndpoints;
using Renta.Application.Common.Request;
using Renta.Application.Common.Response;

namespace Renta.Application.Features.Cars.Query.GetAll;

public record GetAllCarsCommand : ICommand<PagedResponse<GetAllCarsResponse>>
{
    public QueryRequest queryRequest { get; init; } = null!;
}
