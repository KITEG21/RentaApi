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
            // Check calendar for blocked time slots
            var calendarReadRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
            var hasConflict = await calendarReadRepo.GetAll()
                .Where(c => c.YachtId == booking.YachtId 
                    && c.Date.Date == command.Date.Date
                    && c.Status == CalendarStatus.Blocked)
                .AnyAsync(c => 
                    // Check for time overlap
                    TimeOnly.FromTimeSpan(command.StartTime) < c.EndTime && TimeOnly.FromTimeSpan(command.EndTime) > c.StartTime,
                    ct);

            if (hasConflict)
            {
                ThrowError("The yacht is not available for the selected date and time.", 409);
            }

            // Check for existing bookings
            var bookingReadRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
            var existingBooking = await bookingReadRepo.GetAll()
                .Where(b => b.YachtId == booking.YachtId 
                    && b.Id != command.Id
                    && b.Date.Date == command.Date.Date
                    && (b.BookingStatus == BookingStatus.Pending || b.BookingStatus == BookingStatus.Confirmed))
                .AnyAsync(b => 
                    command.StartTime < b.EndTime && command.EndTime > b.StartTime,
                    ct);

            if (existingBooking)
            {
                ThrowError("There is already a booking for this yacht at the selected time.", 409);
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
