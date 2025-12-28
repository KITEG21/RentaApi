using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

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

        // If booking is cancelled, delete the calendar entry
        if (command.BookingStatus == BookingStatus.Cancelled)
        {
            var calendarReadRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
            var calendarEntry = await calendarReadRepo.GetAll()
                .FirstOrDefaultAsync(c => c.BookingId == booking.Id, ct);

            if (calendarEntry != null)
            {
                var calendarWriteRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();
                calendarWriteRepo.Delete(calendarEntry);
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
