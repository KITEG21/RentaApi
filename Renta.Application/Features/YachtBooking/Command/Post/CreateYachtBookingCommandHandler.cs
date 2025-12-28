using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtBooking.Command.Post;

public class CreateYachtBookingCommandHandler : CoreCommandHandler<CreateYachtBookingCommand, CreateYachtBookingResponse>
{
    public CreateYachtBookingCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<CreateYachtBookingResponse> ExecuteAsync(CreateYachtBookingCommand command, CancellationToken ct = default)
    {
        var clientId = CurrentUserId;
        
        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(command.YachtId);
        
        if (yacht is null)
        {
            ThrowError($"Yacht with ID {command.YachtId} not found.", 404);
        }

        // Check yacht calendar for blocked time slots
        var calendarRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
        var hasConflict = await calendarRepo.GetAll()
            .Where(c => c.YachtId == command.YachtId 
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

        // Check for existing bookings with same time slot
        var bookingReadRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var existingBooking = await bookingReadRepo.GetAll()
            .Where(b => b.YachtId == command.YachtId 
                && b.Date.Date == command.Date.Date
                && (b.BookingStatus == BookingStatus.Pending || b.BookingStatus == BookingStatus.Confirmed))
            .AnyAsync(b => 
                // Check for time overlap
                command.StartTime < b.EndTime && command.EndTime > b.StartTime,
                ct);

        if (existingBooking)
        {
            ThrowError("There is already a booking for this yacht at the selected time.", 409);
        }

        var bookingWriteRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();

        // Create booking
        var booking = new Domain.Entities.Bookings.YachtBooking
        {
            YachtId = command.YachtId,
            ClientId = clientId.Value,
            Date = command.Date,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            AdditionalServices = command.AdditionalServices,
            TotalPrice = command.TotalPrice,
            PaymentStatus = PaymentStatus.Pending,
            BookingStatus = BookingStatus.Pending
        };

        await bookingWriteRepo.SaveAsync(booking, false);

        // Save all changes
        await UnitOfWork.SaveChangesAsync();

        return new CreateYachtBookingResponse
        {
            Id = booking.Id,
            YachtId = booking.YachtId,
            ClientId = booking.ClientId,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            TotalPrice = booking.TotalPrice,
            PaymentStatus = booking.PaymentStatus.ToString(),
            BookingStatus = booking.BookingStatus.ToString()
        };
    }
}
