using System;
using Renta.Domain.Enums;

// Create: Renta.Domain/Interfaces/Services/IPaymentService.cs
namespace Renta.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(
        decimal amount,
        string currency,
        string description,
        PaymentMethod method,
        CancellationToken ct = default);

    Task<PaymentResult> RefundPaymentAsync(
        string transactionId,
        decimal amount,
        CancellationToken ct = default);

    Task<PaymentStatus> GetPaymentStatusAsync(
        string transactionId,
        CancellationToken ct = default);
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime ProcessedAt { get; set; }
}
