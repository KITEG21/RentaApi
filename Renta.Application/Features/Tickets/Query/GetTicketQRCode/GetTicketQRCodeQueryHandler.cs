using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Services;

namespace Renta.Application.Features.Tickets.Query.GetTicketQRCode;

public class GetTicketQRCodeQueryHandler : CoreQueryHandler<GetTicketQRCodeQuery, byte[]>
{
    private readonly IQRCodeService _qrCodeService;
    private readonly IActiveUserSession _activeUserSession;
    
    public GetTicketQRCodeQueryHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        IQRCodeService qrCodeService) : base(activeUserSession, unitOfWork)
    {
        _qrCodeService = qrCodeService;
        _activeUserSession = activeUserSession;
    }

    public override async Task<byte[]> ExecuteAsync(GetTicketQRCodeQuery query, CancellationToken ct = default)
    {
        var ticketRepo = UnitOfWork!.ReadDbRepository<Ticket>();
        var ticket = await ticketRepo.GetAll()
            .FirstOrDefaultAsync(t => t.Id == query.TicketId, ct);
            
        if (ticket == null)
        {
            throw new Exception("Ticket not found");
        }
        
        // Verify ownership (unless admin)
        if (!UserRoles.Contains("Admin") && ticket.ClientId != CurrentUserId)
        {
            throw new UnauthorizedAccessException("You don't have access to this ticket");
        }
        
        return _qrCodeService.GenerateQRCodeImage(ticket.QRCode);
    }
}