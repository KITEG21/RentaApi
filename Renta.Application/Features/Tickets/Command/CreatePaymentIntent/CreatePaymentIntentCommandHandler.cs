using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Events;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Settings;
using Stripe;

namespace Renta.Application.Features.Tickets.Command.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler : CoreCommandHandler<CreatePaymentIntentCommand, CreatePaymentIntentResponse>
{
    private readonly StripeSettings _stripeSettings;
    
    public CreatePaymentIntentCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        IOptions<StripeSettings> stripeSettings) : base(activeUserSession, unitOfWork)
    {
        _stripeSettings = stripeSettings.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public override async Task<CreatePaymentIntentResponse> ExecuteAsync(
        CreatePaymentIntentCommand command, 
        CancellationToken ct = default)
    {
        var clientId = CurrentUserId;
        if (!clientId.HasValue)
        {
            ThrowError("User not authenticated", 401);
        }

        var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
        var eventEntity = await eventRepo.GetAll()
            .FirstOrDefaultAsync(e => e.Id == command.EventId, ct);

        if (eventEntity == null)
        {
            ThrowError("Event not found", 404);
        }

        decimal price = command.TicketType switch
        {
            TicketType.General => eventEntity.GeneralTicketPrice,
            TicketType.VIP => eventEntity.VIPTicketPrice,
            TicketType.Backstage => eventEntity.BackstageTicketPrice,
            _ => throw new ArgumentException("Invalid ticket type")
        };

        // Create Stripe PaymentIntent
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(price * 100), // Convert to cents
            Currency = "usd",
            Description = $"Ticket for {eventEntity.Title} - {command.TicketType}",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
            Metadata = new Dictionary<string, string>
            {
                { "event_id", command.EventId.ToString() },
                { "client_id", clientId.Value.ToString() },
                { "ticket_type", command.TicketType.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options, cancellationToken: ct);

        return new CreatePaymentIntentResponse
        {
            ClientSecret = paymentIntent.ClientSecret,
            PaymentIntentId = paymentIntent.Id,
            Amount = price
        };
    }
}