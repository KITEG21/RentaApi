using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.YachtBooking.Command.Patch;

public class PatchPaymentStatusCommandHandler : CoreCommandHandler<PatchPaymentStatusCommand, PatchPaymentStatusResponse>
{
    public PatchPaymentStatusCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<PatchPaymentStatusResponse> ExecuteAsync(PatchPaymentStatusCommand command, CancellationToken ct = default)
    {
        // Only admin or dealer can update payment status
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            ThrowError("You don't have permission to update payment status.", 403);
        }

        var bookingRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var booking = await bookingRepo.GetByIdAsync(command.Id);

        if (booking is null)
        {
            ThrowError($"Booking with ID {command.Id} not found.", 404);
        }

        booking.PaymentStatus = command.PaymentStatus;
        await bookingRepo.UpdateAsync(booking, true);

        return new PatchPaymentStatusResponse
        {
            Id = booking.Id,
            PaymentStatus = booking.PaymentStatus.ToString()
        };
    }
}
