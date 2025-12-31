using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;
using Serilog;

namespace Renta.Application.Features.YachtCalendar.Command.SetAvailability;

public class SetYachtCalendarCommandHandler : CoreCommandHandler<SetYachtCalendarCommand, SetYachtCalendarResponse>
{
    private readonly ILogger _logger;

    public SetYachtCalendarCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
        _logger = Log.ForContext<SetYachtCalendarCommandHandler>();
    }

    public override async Task<SetYachtCalendarResponse> ExecuteAsync(SetYachtCalendarCommand command, CancellationToken ct = default)
    {
        _logger.Information("Setting yacht calendar availability for YachtId: {YachtId}, Date: {Date}, StartTime: {StartTime}, EndTime: {EndTime}, Status: {Status}", command.YachtId, command.Date, command.StartTime, command.EndTime, command.Status);

        // Only Admin, Dealer can manage calendar
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            _logger.Warning("Access denied: User {UserId} attempted to set calendar availability", CurrentUserId);
            ThrowError("You don't have permission to manage yacht calendar.", 403);
        }

        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(command.YachtId);

        if (yacht is null)
        {
            _logger.Warning("Yacht not found: {YachtId}", command.YachtId);
            ThrowError($"Yacht with ID {command.YachtId} not found.", 404);
        }

        // Validate times
        if (command.StartTime >= command.EndTime)
        {
            _logger.Warning("Invalid time range: StartTime {StartTime} >= EndTime {EndTime}", command.StartTime, command.EndTime);
            ThrowError("Start time must be before end time.", 400);
        }

        if (command.Date.Date < DateTime.UtcNow.Date)
        {
            _logger.Warning("Attempted to set availability for past date: {Date}", command.Date);
            ThrowError("Cannot set availability for past dates.", 400);
        }

        // Check if there are existing bookings in this time slot
        var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var hasBookings = await bookingRepo.GetAll()
            .Where(b => b.YachtId == command.YachtId
                && b.Date.Date == command.Date.Date
                && (b.BookingStatus == BookingStatus.Pending || b.BookingStatus == BookingStatus.Confirmed)
                && b.StartTime < command.EndTime
                && b.EndTime > command.StartTime)
            .AnyAsync(ct);

        if (hasBookings)
        {
            _logger.Warning("Cannot block time slot with existing bookings for YachtId: {YachtId}, Date: {Date}", command.YachtId, command.Date);
            ThrowError("Cannot block time slot with existing bookings. Cancel bookings first.", 409);
        }

        var calendarRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();

        // Check if calendar entry already exists for this exact slot
        var existingEntry = await calendarRepo.GetAll()
            .FirstOrDefaultAsync(c => c.YachtId == command.YachtId
                && c.Date.Date == command.Date.Date
                && c.StartTime == TimeOnly.FromTimeSpan(command.StartTime)
                && c.EndTime == TimeOnly.FromTimeSpan(command.EndTime), ct);

        if (existingEntry != null)
        {
            // Update existing entry
            existingEntry.Status = command.Status;
            existingEntry.Reason = command.Reason;
            await calendarRepo.UpdateAsync(existingEntry, true);

            _logger.Information("Updated existing calendar entry: {EntryId}", existingEntry.Id);

            return new SetYachtCalendarResponse
            {
                Id = existingEntry.Id,
                YachtId = existingEntry.YachtId,
                Date = existingEntry.Date,
                StartTime = existingEntry.StartTime.ToTimeSpan(),
                EndTime = existingEntry.EndTime.ToTimeSpan(),
                Status = existingEntry.Status.ToString(),
                Reason = existingEntry.Reason,
                Message = "Calendar entry updated successfully"
            };
        }

        // Create new calendar entry
        var calendarEntry = new YachtCalendarEntity
        {
            YachtId = command.YachtId,
            Date = command.Date,
            StartTime = TimeOnly.FromTimeSpan(command.StartTime),
            EndTime = TimeOnly.FromTimeSpan(command.EndTime),
            Status = command.Status,
            Reason = command.Reason
        };

        await calendarRepo.SaveAsync(calendarEntry, true);

        _logger.Information("Created new calendar entry: {EntryId}", calendarEntry.Id);

        return new SetYachtCalendarResponse
        {
            Id = calendarEntry.Id,
            YachtId = calendarEntry.YachtId,
            Date = calendarEntry.Date,
            StartTime = calendarEntry.StartTime.ToTimeSpan(),
            EndTime = calendarEntry.EndTime.ToTimeSpan(),
            Status = calendarEntry.Status.ToString(),
            Reason = calendarEntry.Reason,
            Message = "Calendar entry created successfully"
        };
    }
}
