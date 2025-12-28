using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtCalendar.Command.BulkSetAvailability;

public class BulkSetYachtCalendarCommandHandler : CoreCommandHandler<BulkSetYachtCalendarCommand, BulkSetYachtCalendarResponse>
{
    public BulkSetYachtCalendarCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<BulkSetYachtCalendarResponse> ExecuteAsync(BulkSetYachtCalendarCommand command, CancellationToken ct = default)
    {
        // Only Admin, Dealer can manage calendar
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            ThrowError("You don't have permission to manage yacht calendar.", 403);
        }

        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(command.YachtId);

        if (yacht is null)
        {
            ThrowError($"Yacht with ID {command.YachtId} not found.", 404);
        }

        // Validate
        if (command.StartTime >= command.EndTime)
        {
            ThrowError("Start time must be before end time.", 400);
        }

        if (command.StartDate.Date < DateTime.UtcNow.Date)
        {
            ThrowError("Start date cannot be in the past.", 400);
        }

        if (command.EndDate < command.StartDate)
        {
            ThrowError("End date must be after start date.", 400);
        }

        var calendarRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();
        var createdEntries = new List<YachtCalendarEntity>();
        var updatedEntries = new List<YachtCalendarEntity>();
        var skippedDates = new List<DateTime>();

        // Iterate through dates
        for (var date = command.StartDate.Date; date <= command.EndDate.Date; date = date.AddDays(1))
        {
            // Check if this day of week should be included
            if (command.DaysOfWeek != null && command.DaysOfWeek.Any() && !command.DaysOfWeek.Contains(date.DayOfWeek))
            {
                continue;
            }

            // Check for existing bookings
            var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
            var hasBookings = await bookingRepo.GetAll()
                .Where(b => b.YachtId == command.YachtId
                    && b.Date.Date == date.Date
                    && (b.BookingStatus == BookingStatus.Pending || b.BookingStatus == BookingStatus.Confirmed)
                    && b.StartTime < command.EndTime
                    && b.EndTime > command.StartTime)
                .AnyAsync(ct);

            if (hasBookings)
            {
                skippedDates.Add(date);
                continue;
            }

            // Check if entry exists
            var existingEntry = await calendarRepo.GetAll()
                .FirstOrDefaultAsync(c => c.YachtId == command.YachtId
                    && c.Date.Date == date.Date
                    && c.StartTime == TimeOnly.FromTimeSpan(command.StartTime)
                    && c.EndTime == TimeOnly.FromTimeSpan(command.EndTime), ct);

            if (existingEntry != null)
            {
                existingEntry.Status = command.Status;
                existingEntry.Reason = command.Reason;
                await calendarRepo.UpdateAsync(existingEntry, false);
                updatedEntries.Add(existingEntry);
            }
            else
            {
                var newEntry = new YachtCalendarEntity
                {
                    YachtId = command.YachtId,
                    Date = date,
                    StartTime = TimeOnly.FromTimeSpan(command.StartTime),
                    EndTime = TimeOnly.FromTimeSpan(command.EndTime),
                    Status = command.Status,
                    Reason = command.Reason
                };
                await calendarRepo.SaveAsync(newEntry, false);
                createdEntries.Add(newEntry);
            }
        }

        await UnitOfWork!.SaveChangesAsync();

        return new BulkSetYachtCalendarResponse
        {
            YachtId = command.YachtId,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            CreatedCount = createdEntries.Count,
            UpdatedCount = updatedEntries.Count,
            SkippedCount = skippedDates.Count,
            SkippedDates = skippedDates,
            Message = $"Successfully processed {createdEntries.Count + updatedEntries.Count} calendar entries. Skipped {skippedDates.Count} dates with existing bookings."
        };
    }
}
