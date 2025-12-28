// Update: Renta.WebApi/Endpoints/v1/Webhooks/StripeWebhookEndpoint.cs
using System.IO;
using System.Text;
using FastEndpoints;
using Microsoft.Extensions.Options;
using Renta.Application.Features.Tickets.Command.ProcessStripePayment;
using Renta.Domain.Enums;
using Renta.Domain.Settings;
using Stripe;
using StripeEvents = Stripe.Events;


namespace Renta.WebApi.Endpoints.v1.Webhooks;

public class StripeWebhookEndpoint : EndpointWithoutRequest
{
    private readonly StripeSettings _stripeSettings;
    private readonly ILogger<StripeWebhookEndpoint> _logger;

    public StripeWebhookEndpoint(
        IOptions<StripeSettings> stripeSettings,
        ILogger<StripeWebhookEndpoint> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/webhooks/stripe");
        AllowAnonymous();
        //Options(x => x.RequireCors(cors => cors.AllowAnyOrigin()));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Read raw body for signature verification
        HttpContext.Request.EnableBuffering();
        string json;
        
        using (var reader = new StreamReader(
            HttpContext.Request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true))
        {
            json = await reader.ReadToEndAsync();
            HttpContext.Request.Body.Position = 0;
        }

        var stripeSignature = HttpContext.Request.Headers["Stripe-Signature"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(stripeSignature))
        {
            _logger.LogWarning("Webhook received without Stripe-Signature header");
            await Send.OkAsync("Missing signature", ct);
            return;
        }

        try
        {
            // Verify webhook signature
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                _stripeSettings.WebhookSecret,
                throwOnApiVersionMismatch: false
            );

            _logger.LogInformation("Stripe webhook received: {EventType}", stripeEvent.Type);

            // Route to appropriate handler
            await RouteEvent(stripeEvent, ct);

            await Send.OkAsync("Webhook processed successfully", ct);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe signature verification failed");
            await Send.OkAsync("Invalid signature", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook");
            await Send.OkAsync("Webhook processing error", ct);
        }
    }

    private async Task RouteEvent(Event stripeEvent, CancellationToken ct)
    {
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                if (stripeEvent.Data.Object is PaymentIntent paymentIntentSuccess)
                {
                    await HandlePaymentSuccess(paymentIntentSuccess, ct);
                }
                break;

            case "payment_intent.payment_failed":
                if (stripeEvent.Data.Object is PaymentIntent paymentIntentFailed)
                {
                    await HandlePaymentFailure(paymentIntentFailed, ct);
                }
                break;

            case "payment_intent.canceled":
                if (stripeEvent.Data.Object is PaymentIntent paymentIntentCanceled)
                {
                    await HandlePaymentCanceled(paymentIntentCanceled, ct);
                }
                break;

            default:
                _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                break;
        }
    }

    private async Task HandlePaymentSuccess(PaymentIntent paymentIntent, CancellationToken ct)
    {
        try
        {
            // Validate metadata
            if (!ValidateMetadata(paymentIntent, out var eventId, out var clientId, out var ticketType))
            {
                _logger.LogWarning("Payment intent {PaymentIntentId} has invalid or missing metadata", paymentIntent.Id);
                return;
            }

            // Create command and execute via CQRS
            var command = new ProcessStripePaymentCommand
            {
                PaymentIntentId = paymentIntent.Id,
                AmountInCents = paymentIntent.Amount,
                EventId = eventId,
                ClientId = clientId,
                TicketType = ticketType
            };

            var result = await command.ExecuteAsync(ct);

            if (result.Success)
            {
                _logger.LogInformation(
                    "Payment processed successfully. Ticket {TicketId} created for payment intent {PaymentIntentId}",
                    result.TicketId,
                    paymentIntent.Id);
            }
            else
            {
                _logger.LogError(
                    "Failed to process payment intent {PaymentIntentId}: {Error}",
                    paymentIntent.Id,
                    result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling payment success for payment intent {PaymentIntentId}", paymentIntent.Id);
        }
    }

    private async Task HandlePaymentFailure(PaymentIntent paymentIntent, CancellationToken ct)
    {
        _logger.LogWarning(
            "Payment failed for payment intent {PaymentIntentId}: {Error}",
            paymentIntent.Id,
            paymentIntent.LastPaymentError?.Message ?? "Unknown error");

        // TODO: Implement failure notification command/handler
        await Task.CompletedTask;
    }

    private async Task HandlePaymentCanceled(PaymentIntent paymentIntent, CancellationToken ct)
    {
        _logger.LogInformation("Payment canceled for payment intent {PaymentIntentId}", paymentIntent.Id);
        
        // TODO: Implement cancellation handler
        await Task.CompletedTask;
    }

    private bool ValidateMetadata(
        PaymentIntent paymentIntent,
        out Guid eventId,
        out Guid clientId,
        out TicketType ticketType)
    {
        eventId = Guid.Empty;
        clientId = Guid.Empty;
        ticketType = TicketType.General;

        if (paymentIntent?.Metadata == null
            || !paymentIntent.Metadata.ContainsKey("event_id")
            || !paymentIntent.Metadata.ContainsKey("client_id")
            || !paymentIntent.Metadata.ContainsKey("ticket_type"))
        {
            return false;
        }

        return Guid.TryParse(paymentIntent.Metadata["event_id"], out eventId)
            && Guid.TryParse(paymentIntent.Metadata["client_id"], out clientId)
            && Enum.TryParse(paymentIntent.Metadata["ticket_type"], out ticketType);
    }
}