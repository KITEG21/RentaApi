using Microsoft.EntityFrameworkCore;
using Renta.Domain.Entities.Bookings;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtBooking.Query.GetAvailability;

public class GetYachtAvailabilityQueryHandler : CoreQueryHandler<GetYachtAvailabilityQuery, GetYachtAvailabilityResponse>
{
    public GetYachtAvailabilityQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetYachtAvailabilityResponse> ExecuteAsync(GetYachtAvailabilityQuery query, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(query.YachtId);
        
        if (yacht is null)
        {
            ThrowError($"Yacht with ID {query.YachtId} not found.", 404);
        }

        var calendarRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
        var unavailableSlots = await calendarRepo.GetAll()
            .Where(c => c.YachtId == query.YachtId 
                && c.Date.Date == query.Date.Date
                && c.Status == CalendarStatus.Blocked)
            .Select(c => new UnavailableTimeSlot
            {
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                Status = c.Status.ToString()
            })
            .ToListAsync(ct);

        return new GetYachtAvailabilityResponse
        {
            YachtId = query.YachtId,
            YachtName = yacht.Name,
            Date = query.Date,
            UnavailableSlots = unavailableSlots,
            IsFullyBooked = unavailableSlots.Any()
        };
    }
}
