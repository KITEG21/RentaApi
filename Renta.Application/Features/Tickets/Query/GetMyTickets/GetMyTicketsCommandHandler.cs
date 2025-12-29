using Microsoft.EntityFrameworkCore;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Tickets.Query.GetMyTickets;

public class GetMyTicketsCommandHandler : CoreQueryHandler<GetMyTicketsCommand, PagedResponse<GetMyTicketsResponse>>
{
    public GetMyTicketsCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<PagedResponse<GetMyTicketsResponse>> ExecuteAsync(GetMyTicketsCommand command, CancellationToken ct = default)
    {
        var ticketRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Ticket>();
        var ticketsQuery = ticketRepo.GetAllFiltered(req: command.queryRequest);

        var clientId = CurrentUserId;
        if (!clientId.HasValue)
        {
            ThrowError("User not authenticated", 401);
        }

        // Filter by current user's tickets
        ticketsQuery = ticketsQuery.Where(t => t.ClientId == clientId.Value);

        var response = await ticketsQuery
            .Include(t => t.Event)
            .Select(ticket => new GetMyTicketsResponse
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                EventTitle = ticket.Event.Title,
                EventDate = ticket.Event.EventDate,
                EventLocation = ticket.Event.Location,
                TicketType = ticket.TicketType,
                PricePaid = ticket.PricePaid,
                QRCode = ticket.QRCode,
                Status = ticket.Status,
                PurchaseDate = ticket.PurchaseDate
            })
            .ToPagedResultAsync(command.queryRequest.Page, command.queryRequest.PerPage);

        return response;
    }
}
