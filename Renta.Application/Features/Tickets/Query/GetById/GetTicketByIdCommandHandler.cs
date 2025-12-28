using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Tickets.Query.GetById;

public class GetTicketByIdCommandHandler : CoreQueryHandler<GetTicketByIdCommand, GetTicketByIdResponse>
{
    public GetTicketByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetTicketByIdResponse> ExecuteAsync(GetTicketByIdCommand command, CancellationToken ct = default)
    {
        var ticketRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Ticket>();
        
        var ticket = await ticketRepo
            .GetAll(includes: new Expression<Func<Domain.Entities.Events.Ticket, object>>[] { t => t.Event, t => t.Client })
            .Where(t => t.Id == command.Id)
            .Select(t => new GetTicketByIdResponse
            {
                Id = t.Id,
                EventId = t.EventId,
                EventTitle = t.Event.Title,
                EventDate = t.Event.EventDate,
                EventLocation = t.Event.Location,
                ClientId = t.ClientId,
                ClientName = t.Client.FirstName + " " + t.Client.LastName,
                ClientEmail = t.Client.Email ?? string.Empty,
                TicketType = t.TicketType,
                PricePaid = t.PricePaid,
                QRCode = t.QRCode,
                Status = t.Status,
                PurchaseDate = t.PurchaseDate
            })
            .FirstOrDefaultAsync(ct);

        if (ticket == null)
        {
            throw new Exception("Ticket not found");
        }

        return ticket;
    }
}
