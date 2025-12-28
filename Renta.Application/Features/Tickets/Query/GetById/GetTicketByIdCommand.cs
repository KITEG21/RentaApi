using System;
using FastEndpoints;

namespace Renta.Application.Features.Tickets.Query.GetById;

public record GetTicketByIdCommand : ICommand<GetTicketByIdResponse>
{
    public Guid Id { get; init; }
}
