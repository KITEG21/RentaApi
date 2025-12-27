using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.YachtBooking.Command.Delete;

public class CancelYachtBookingCommandHandler : CoreCommandHandler<CancelYachtBookingCommand, CancelYachtBookingResponse>
{
    public CancelYachtBookingCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<CancelYachtBookingResponse> ExecuteAsync(CancelYachtBookingCommand command, CancellationToken ct = default)
    {
        var clientId = CurrentUserId;
        
        if (clientId is null || clientId == Guid.Empty)
        {
            ThrowError("User not authenticated", 401);
        }

        var bookingRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var booking = await bookingRepo.GetByIdAsync(command.Id);

        if (booking is null)
        {
            ThrowError($"Booking with ID {command.Id} not found.", 404);
        }

        // Only allow client to cancel their own bookings (unless admin)
        if (booking.ClientId != clientId.Value && !UserRoles.Contains("Admin"))
        {
            ThrowError("You don't have permission to cancel this booking.", 403);
        }

        // Don't allow cancelling completed bookings
        if (booking.BookingStatus == BookingStatus.Completed)
        {
            ThrowError("Cannot cancel completed bookings.", 400);
        }

        if (booking.BookingStatus == BookingStatus.Cancelled)
        {
            ThrowError("This booking is already cancelled.", 400);
        }

        // Update booking status
        booking.BookingStatus = BookingStatus.Cancelled;
        await bookingRepo.UpdateAsync(booking, false);

        // Release calendar slot
        var calendarRepo = UnitOfWork!.WriteDbRepository<YachtCalendar>();
        var calendarEntry = await calendarRepo.GetAll()
            .FirstOrDefaultAsync(c => c.YachtId == booking.YachtId 
                && c.Date == booking.Date
                && c.StartTime == TimeOnly.FromTimeSpan(booking.StartTime)
                && c.EndTime == TimeOnly.FromTimeSpan(booking.EndTime)
                && c.Status == CalendarStatus.Reserved, ct);

        if (calendarEntry != null)
        {
            await calendarRepo.DeleteAsync(calendarEntry, false);
        }

        await UnitOfWork!.SaveChangesAsync();

        return new CancelYachtBookingResponse
        {
            Id = booking.Id,
            BookingStatus = booking.BookingStatus.ToString(),
            Message = "Booking cancelled successfully"
        };
    }
}
