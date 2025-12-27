using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.YachtBooking.Command.Patch;

public class PatchBookingStatusCommandHandler : CoreCommandHandler<PatchBookingStatusCommand, PatchBookingStatusResponse>
{
    public PatchBookingStatusCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<PatchBookingStatusResponse> ExecuteAsync(PatchBookingStatusCommand command, CancellationToken ct = default)
    {
        // Only admin or dealer can update booking status
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            ThrowError("You don't have permission to update booking status.", 403);
        }

        var bookingRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var booking = await bookingRepo.GetByIdAsync(command.Id);

        if (booking is null)
        {
            ThrowError($"Booking with ID {command.Id} not found.", 404);
        }

        var oldStatus = booking.BookingStatus;
        booking.BookingStatus = command.BookingStatus;
        await bookingRepo.UpdateAsync(booking, false);

        // If booking is cancelled, release the calendar slot
        if (command.BookingStatus == BookingStatus.Cancelled)
        {
            var calendarRepo = UnitOfWork!.WriteDbRepository<YachtCalendar>();
            var calendarEntry = await calendarRepo.GetAll()
                .FirstOrDefaultAsync(c => c.YachtId == booking.YachtId 
                    && c.Date == booking.Date
                    && c.StartTime == TimeOnly.FromTimeSpan(booking.StartTime)
                    && c.EndTime == TimeOnly.FromTimeSpan(booking.EndTime)
                    && c.Status == CalendarStatus.Reserved, ct);

            if (calendarEntry != null)
            {
                // Change status back to Available or delete the entry
                await calendarRepo.DeleteAsync(calendarEntry, false);
            }
        }

        await UnitOfWork!.SaveChangesAsync();

        return new PatchBookingStatusResponse
        {
            Id = booking.Id,
            BookingStatus = booking.BookingStatus.ToString()
        };
    }
}
