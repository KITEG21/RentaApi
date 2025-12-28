using Microsoft.EntityFrameworkCore;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtCalendar.Query.GetCalendar;

public class GetYachtCalendarQueryHandler : CoreQueryHandler<GetYachtCalendarQuery, GetYachtCalendarResponse>
{
    public GetYachtCalendarQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetYachtCalendarResponse> ExecuteAsync(GetYachtCalendarQuery query, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(query.YachtId);

        if (yacht is null)
        {
            ThrowError($"Yacht with ID {query.YachtId} not found.", 404);
        }

        if (query.EndDate < query.StartDate)
        {
            ThrowError("End date must be after start date.", 400);
        }

        var calendarRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
        var entries = await calendarRepo.GetAll()
            .Where(c => c.YachtId == query.YachtId
                && c.Date.Date >= query.StartDate.Date
                && c.Date.Date <= query.EndDate.Date)
            .OrderBy(c => c.Date)
            .ThenBy(c => c.StartTime)
            .Select(c => new CalendarEntryDto
            {
                Id = c.Id,
                Date = c.Date,
                StartTime = c.StartTime.ToTimeSpan(),
                EndTime = c.EndTime.ToTimeSpan(),
                Status = c.Status.ToString(),
                Reason = c.Reason
            })
            .ToListAsync(ct);

        return new GetYachtCalendarResponse
        {
            YachtId = query.YachtId,
            YachtName = yacht.Name,
            StartDate = query.StartDate,
            EndDate = query.EndDate,
            Entries = entries
        };
    }
}
