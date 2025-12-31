using Microsoft.EntityFrameworkCore;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Tickets.Query.GetMyTickets;

public class GetMyTicketsCommandHandler : CoreQueryHandler<GetMyTicketsCommand, PagedResponse<GetMyTicketsResponse>>
{
    private readonly ILogger _logger;

    public GetMyTicketsCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
        _logger = Log.ForContext<GetMyTicketsCommandHandler>();
    }

    public override async Task<PagedResponse<GetMyTicketsResponse>> ExecuteAsync(GetMyTicketsCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving my tickets for user: {UserId}", CurrentUserId);

        var ticketRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Ticket>();
        var ticketsQuery = ticketRepo.GetAllFiltered(req: command.queryRequest);

        var clientId = CurrentUserId;
        if (!clientId.HasValue)
        {
            _logger.Warning("User not authenticated for getting tickets");
            ThrowError("User not authenticated", 401);
        }

        // Filter by current user's tickets
        ticketsQuery = ticketsQuery.Where(t => t.ClientId == clientId.Value);

        var response = await ticketsQuery
            .Include(t => t.Event)
            .ToPagedResultAsync(command.queryRequest.Page ?? 1, command.queryRequest.PerPage ?? 10, ticket => new GetMyTicketsResponse
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
            });

        _logger.Information("Successfully retrieved tickets for user: {UserId}", CurrentUserId);

        return response;
    }
}
