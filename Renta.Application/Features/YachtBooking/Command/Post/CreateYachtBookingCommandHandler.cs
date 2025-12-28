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

        // Check yacht calendar for ANY unavailability (blocks OR reservations)
        var calendarRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
        var hasConflict = await calendarRepo.GetAll()
            .Where(c => c.YachtId == command.YachtId 
                && c.Date.Date == command.Date.Date)
            .AnyAsync(c => 
                // Check for time overlap
                TimeOnly.FromTimeSpan(command.StartTime) < c.EndTime && TimeOnly.FromTimeSpan(command.EndTime) > c.StartTime,
                ct);

        if (hasConflict)
        {
            ThrowError("The yacht is not available for the selected date and time.", 409);
        }

        var bookingWriteRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var calendarWriteRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();

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

        // Create corresponding calendar entry with Reserved status
        var calendarEntry = new YachtCalendarEntity
        {
            YachtId = command.YachtId,
            Date = command.Date,
            StartTime = TimeOnly.FromTimeSpan(command.StartTime),
            EndTime = TimeOnly.FromTimeSpan(command.EndTime),
            Status = CalendarStatus.Reserved,
            Reason = "Client Booking",
            BookingId = booking.Id
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
