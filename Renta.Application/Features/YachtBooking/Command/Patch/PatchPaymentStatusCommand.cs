using FastEndpoints;
using Renta.Domain.Enums;

namespace Renta.Application.Features.YachtBooking.Command.Patch;

public record PatchPaymentStatusCommand : ICommand<PatchPaymentStatusResponse>
{
    public Guid Id { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
