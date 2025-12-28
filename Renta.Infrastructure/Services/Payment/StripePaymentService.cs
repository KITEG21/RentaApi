using Microsoft.Extensions.Options;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Services;
using Renta.Domain.Settings;
using Stripe;

namespace Renta.Infrastructure.Services.Payment;

public class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;

    public StripePaymentService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(
        decimal amount,
        string currency,
        string description,
        Domain.Enums.PaymentMethod method,
        CancellationToken ct = default)
    {
        try
        {
            // Create a PaymentIntent (modern Stripe approach)
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe uses cents
                Currency = currency.ToLower(),
                Description = description,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                Metadata = new Dictionary<string, string>
                {
                    { "payment_method", method.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options, cancellationToken: ct);

            // For immediate charges (requires payment method)
            // If you have a payment method token, confirm the payment:
            // var confirmOptions = new PaymentIntentConfirmOptions();
            // paymentIntent = await service.ConfirmAsync(paymentIntent.Id, confirmOptions, cancellationToken: ct);

            return new PaymentResult
            {
                Success = paymentIntent.Status == "succeeded" || paymentIntent.Status == "requires_capture",
                TransactionId = paymentIntent.Id,
                Status = MapStripeStatus(paymentIntent.Status),
                ProcessedAt = DateTime.UtcNow,
                ErrorMessage = paymentIntent.Status != "succeeded" ? 
                    $"Payment status: {paymentIntent.Status}" : null
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                Status = Domain.Enums.PaymentStatus.Cancelled,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<PaymentResult> RefundPaymentAsync(
        string transactionId,
        decimal amount,
        CancellationToken ct = default)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = transactionId,
                Amount = (long)(amount * 100)
            };

            var service = new RefundService();
            var refund = await service.CreateAsync(options, cancellationToken: ct);

            return new PaymentResult
            {
                Success = refund.Status == "succeeded",
                TransactionId = refund.Id,
                Status = refund.Status == "succeeded" ? 
                    Domain.Enums.PaymentStatus.Refunded : 
                    Domain.Enums.PaymentStatus.Pending,
                ProcessedAt = DateTime.UtcNow
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                Status = Domain.Enums.PaymentStatus.Cancelled,
                ProcessedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<Domain.Enums.PaymentStatus> GetPaymentStatusAsync(
        string transactionId,
        CancellationToken ct = default)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(transactionId, cancellationToken: ct);
            return MapStripeStatus(paymentIntent.Status);
        }
        catch (StripeException)
        {
            return Domain.Enums.PaymentStatus.Cancelled;
        }
    }

    private Domain.Enums.PaymentStatus MapStripeStatus(string stripeStatus)
    {
        return stripeStatus switch
        {
            "succeeded" => Domain.Enums.PaymentStatus.Paid,
            "requires_payment_method" => Domain.Enums.PaymentStatus.Pending,
            "requires_confirmation" => Domain.Enums.PaymentStatus.Pending,
            "requires_action" => Domain.Enums.PaymentStatus.Pending,
            "processing" => Domain.Enums.PaymentStatus.Pending,
            "requires_capture" => Domain.Enums.PaymentStatus.PartiallyPaid,
            "canceled" => Domain.Enums.PaymentStatus.Cancelled,
            _ => Domain.Enums.PaymentStatus.Pending
        };
    }
}