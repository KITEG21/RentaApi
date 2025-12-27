using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;

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
        
        if (clientId is null || clientId == Guid.Empty)
        {
            ThrowError("User not authenticated", 401);
        }

        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(command.YachtId);
        
        if (yacht is null)
        {
            ThrowError($"Yacht with ID {command.YachtId} not found.", 404);
        }

        // Check yacht calendar availability
        var calendarRepo = UnitOfWork!.ReadDbRepository<YachtCalendar>();
        var hasConflict = await calendarRepo.GetAll()
            .Where(c => c.YachtId == command.YachtId 
                && c.Date.Date == command.Date.Date
                && c.Status != CalendarStatus.Available)
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
                (command.StartTime < b.EndTime && command.EndTime > b.StartTime),
                ct);

        if (existingBooking)
        {
            ThrowError("There is already a booking for this yacht at the selected time.", 409);
        }

        var bookingWriteRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var calendarWriteRepo = UnitOfWork!.WriteDbRepository<YachtCalendar>();

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

        // Reserve the time slot in calendar
        var calendarEntry = new YachtCalendar
        {
            YachtId = command.YachtId,
            Date = command.Date,
            StartTime = TimeOnly.FromTimeSpan(command.StartTime),
            EndTime = TimeOnly.FromTimeSpan(command.EndTime),
            Status = CalendarStatus.Reserved
        };

        await calendarWriteRepo.SaveAsync(calendarEntry, false);

        // Save all changes in one transaction
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
