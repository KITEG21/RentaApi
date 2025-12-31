using Microsoft.EntityFrameworkCore;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Tickets.Query.GetAll;

public class GetAllTicketsCommandHandler : CoreQueryHandler<GetAllTicketsCommand, PagedResponse<GetAllTicketsResponse>>
{
    private readonly ILogger _logger;

    public GetAllTicketsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<GetAllTicketsCommandHandler>();
    }

    public override async Task<PagedResponse<GetAllTicketsResponse>> ExecuteAsync(GetAllTicketsCommand command, CancellationToken ct = default)
    {
        var ticketRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Ticket>();
        var tickets = ticketRepo.GetAllFiltered(req: command.queryRequest);

        var response = await tickets
            .Include(t => t.Event)
            .Include(t => t.Client)
            .Select(ticket => new GetAllTicketsResponse
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                EventTitle = ticket.Event.Title,
                ClientId = ticket.ClientId,
                ClientName = ticket.Client.FirstName + " " + ticket.Client.LastName,
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
