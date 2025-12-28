using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtBooking.Command.Put;

public class UpdateYachtBookingCommandHandler : CoreCommandHandler<UpdateYachtBookingCommand, UpdateYachtBookingResponse>
{
    public UpdateYachtBookingCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<UpdateYachtBookingResponse> ExecuteAsync(UpdateYachtBookingCommand command, CancellationToken ct = default)
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

        // Don't allow updating confirmed or completed bookings
        if (booking.BookingStatus == BookingStatus.Confirmed || booking.BookingStatus == BookingStatus.Completed)
        {
            ThrowError("Cannot update confirmed or completed bookings.", 400);
        }

        // Check if date/time changed
        bool timeChanged = booking.Date.Date != command.Date.Date 
            || booking.StartTime != command.StartTime 
            || booking.EndTime != command.EndTime;

        if (timeChanged)
        {
            // Check calendar for conflicts (excluding current booking's calendar entry)
            var calendarReadRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
            var hasConflict = await calendarReadRepo.GetAll()
                .Where(c => c.YachtId == booking.YachtId 
                    && c.Date.Date == command.Date.Date
                    && c.BookingId != command.Id) // Exclude current booking's calendar entry
                .AnyAsync(c => 
                    // Check for time overlap
                    TimeOnly.FromTimeSpan(command.StartTime) < c.EndTime && TimeOnly.FromTimeSpan(command.EndTime) > c.StartTime,
                    ct);

            if (hasConflict)
            {
                ThrowError("The yacht is not available for the selected date and time.", 409);
            }

            // Update the calendar entry linked to this booking
            var calendarWriteRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();
            var calendarEntry = await calendarWriteRepo.GetAll()
                .FirstOrDefaultAsync(c => c.BookingId == booking.Id, ct);

            if (calendarEntry != null)
            {
                // Update existing calendar entry
                calendarEntry.Date = command.Date;
                calendarEntry.StartTime = TimeOnly.FromTimeSpan(command.StartTime);
                calendarEntry.EndTime = TimeOnly.FromTimeSpan(command.EndTime);
                await calendarWriteRepo.UpdateAsync(calendarEntry, false);
            }
        }

        // Update booking
        booking.Date = command.Date;
        booking.StartTime = command.StartTime;
        booking.EndTime = command.EndTime;
        booking.AdditionalServices = command.AdditionalServices;
        booking.TotalPrice = command.TotalPrice;

        await bookingRepo.UpdateAsync(booking, false);

        // Save all changes
        await UnitOfWork!.SaveChangesAsync();

        return new UpdateYachtBookingResponse
        {
            Id = booking.Id,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            TotalPrice = booking.TotalPrice,
            PaymentStatus = booking.PaymentStatus.ToString(),
            BookingStatus = booking.BookingStatus.ToString()
        };
    }
}
