using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Events;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Tickets.Command.BuyTicket;

public class BuyTicketCommandHandler : CoreCommandHandler<BuyTicketCommand, BuyTicketResponse>
{
    public BuyTicketCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<BuyTicketResponse> ExecuteAsync(BuyTicketCommand command, CancellationToken ct = default)
    {
        var clientId = CurrentUserId;
        if (!clientId.HasValue)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        // Get the event
        var eventRepo = UnitOfWork!.WriteDbRepository<Event>();
        var eventEntity = await eventRepo
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == command.EventId, ct);

        if (eventEntity == null)
        {
            throw new Exception("Event not found");
        }

        // Check if event is active
        if (eventEntity.Status != EventStatus.Active)
        {
            throw new Exception("Event is not available for ticket purchase");
        }

        // Check if tickets are available
        if (eventEntity.AvailableTickets <= 0)
        {
            throw new Exception("No tickets available for this event");
        }

        // Determine price based on ticket type
        decimal price = command.TicketType switch
        {
            TicketType.General => eventEntity.GeneralTicketPrice,
            TicketType.VIP => eventEntity.VIPTicketPrice,
            TicketType.Backstage => eventEntity.BackstageTicketPrice,
            _ => throw new Exception("Invalid ticket type")
        };

        // Create the ticket
        var ticketRepo = UnitOfWork!.WriteDbRepository<Ticket>();
        var newTicket = new Ticket
        {
            EventId = command.EventId,
            ClientId = clientId.Value,
            TicketType = command.TicketType,
            PricePaid = price,
            QRCode = GenerateQRCode(),
            Status = TicketStatus.Valid,
            PurchaseDate = DateTime.UtcNow
        };

        await ticketRepo.SaveAsync(newTicket, false);

        // Update available tickets
        eventEntity.AvailableTickets -= 1;
        await eventRepo.SaveAsync(eventEntity, false);

        // Commit transaction
        await UnitOfWork.CommitChangesAsync();

        return new BuyTicketResponse
        {
            Id = newTicket.Id,
            EventId = newTicket.EventId,
            EventTitle = eventEntity.Title,
            TicketType = newTicket.TicketType,
            PricePaid = newTicket.PricePaid,
            QRCode = newTicket.QRCode,
            Status = newTicket.Status,
            PurchaseDate = newTicket.PurchaseDate
        };
    }

    private string GenerateQRCode()
    {
        // TODO: Generate a unique QR code
        return Guid.NewGuid().ToString("N").ToUpper();
    }
}
