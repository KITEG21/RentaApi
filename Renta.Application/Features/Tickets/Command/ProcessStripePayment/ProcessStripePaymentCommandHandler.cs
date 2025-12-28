using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Events;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Services;

namespace Renta.Application.Features.Tickets.Command.ProcessStripePayment;

public class ProcessStripePaymentCommandHandler : CoreCommandHandler<ProcessStripePaymentCommand, ProcessStripePaymentResponse>
{
    private readonly IQRCodeService _qrCodeService;
    private readonly IEmailService _emailService;

    public ProcessStripePaymentCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        IQRCodeService qrCodeService,
        IEmailService emailService) : base(activeUserSession, unitOfWork)
    {
        _qrCodeService = qrCodeService;
        _emailService = emailService;
    }

    public override async Task<ProcessStripePaymentResponse> ExecuteAsync(
        ProcessStripePaymentCommand command,
        CancellationToken ct = default)
    {
        try
        {
            // Check if ticket already exists (idempotency)
            var ticketRepo = UnitOfWork!.WriteDbRepository<Ticket>();
            var existingTicket = await ticketRepo.GetAll()
                .FirstOrDefaultAsync(t => t.PaymentTransactionId == command.PaymentIntentId, ct);

            if (existingTicket != null)
            {
                return new ProcessStripePaymentResponse
                {
                    Success = true,
                    TicketId = existingTicket.Id,
                    QRCode = existingTicket.QRCode,
                    ErrorMessage = "Ticket already exists"
                };
            }

            // Get event
            var eventRepo = UnitOfWork!.WriteDbRepository<Event>();
            var eventEntity = await eventRepo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == command.EventId, ct);

            if (eventEntity == null)
            {
                return new ProcessStripePaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Event not found"
                };
            }

            // Check availability
            if (eventEntity.AvailableTickets <= 0)
            {
                return new ProcessStripePaymentResponse
                {
                    Success = false,
                    ErrorMessage = "No tickets available"
                };
            }

            // Create ticket
            var newTicket = new Ticket
            {
                EventId = command.EventId,
                ClientId = command.ClientId,
                TicketType = command.TicketType,
                PricePaid = command.AmountInCents / 100m,
                QRCode = _qrCodeService.GenerateQRCodeString(Guid.NewGuid(), command.EventId, command.ClientId),
                Status = TicketStatus.Valid,
                PurchaseDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Paid,
                PaymentTransactionId = command.PaymentIntentId,
                PaymentMethod = "Stripe",
                PaymentDate = DateTime.UtcNow
            };

            await ticketRepo.SaveAsync(newTicket, false);

            // Update event inventory
            eventEntity.AvailableTickets -= 1;
            await eventRepo.UpdateAsync(eventEntity, false);

            // Commit transaction
            await UnitOfWork.CommitChangesAsync();

            // Send confirmation email (fire and forget - don't block webhook response)
            _ = Task.Run(async () =>
            {
                try
                {
                    var userRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Identity.User>();
                    var user = await userRepo.GetAll()
                        .FirstOrDefaultAsync(u => u.Id == command.ClientId, ct);

                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await SendConfirmationEmail(user, eventEntity, newTicket);
                    }
                }
                catch
                {
                    // Log error but don't fail the webhook
                }
            }, ct);

            return new ProcessStripePaymentResponse
            {
                Success = true,
                TicketId = newTicket.Id,
                QRCode = newTicket.QRCode
            };
        }
        catch (Exception ex)
        {
            return new ProcessStripePaymentResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task SendConfirmationEmail(
        Domain.Entities.Identity.User user,
        Event eventEntity,
        Ticket ticket)
    {
        var subject = $"Your Ticket for {eventEntity.Title}";
        var body = BuildTicketEmailBody(eventEntity, ticket, user);
        await _emailService.SendEmailAsync(user.Email!, subject, body);
    }

    private string BuildTicketEmailBody(Event eventEntity, Ticket ticket, Domain.Entities.Identity.User user)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .ticket-info {{ background-color: white; padding: 15px; margin: 15px 0; border-left: 4px solid #4CAF50; }}
        .qr-code {{ background-color: white; padding: 20px; text-align: center; margin: 20px 0; border: 2px dashed #4CAF50; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎉 Ticket Confirmed!</h1>
        </div>
        <div class='content'>
            <p>Hello {user.FirstName ?? user.UserName},</p>
            <p>Your ticket purchase was successful! We're excited to see you at the event.</p>
            
            <div class='ticket-info'>
                <h2>Event Details</h2>
                <p><strong>Event:</strong> {eventEntity.Title}</p>
                <p><strong>Date:</strong> {eventEntity.EventDate:MMMM dd, yyyy}</p>
                <p><strong>Time:</strong> {eventEntity.EventDate:h:mm tt}</p>
                <p><strong>Location:</strong> {eventEntity.Location}</p>
                <p><strong>Ticket Type:</strong> {ticket.TicketType}</p>
                <p><strong>Price Paid:</strong> ${ticket.PricePaid:F2}</p>
            </div>
            
            <div class='qr-code'>
                <h3>Your QR Code</h3>
                <p style='font-family: monospace; font-size: 14px; word-break: break-all;'>{ticket.QRCode}</p>
                <p style='font-size: 12px; color: #666;'>Please present this QR code at the event entrance</p>
            </div>
            
            <p><strong>Ticket ID:</strong> {ticket.Id}</p>
            <p><strong>Purchase Date:</strong> {ticket.PurchaseDate:MMMM dd, yyyy h:mm tt}</p>
        </div>
        <div class='footer'>
            <p>Thank you for your purchase!</p>
            <p>If you have any questions, please contact our support team.</p>
        </div>
    </div>
</body>
</html>";
    }
}