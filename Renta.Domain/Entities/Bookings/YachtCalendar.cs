using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Bookings;

public class YachtCalendar: Entity
{
    public Guid YachtId { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public CalendarStatus Status { get; set; } = CalendarStatus.Blocked;
    public string Reason { get; set; } = string.Empty;
    public Yacht Yacht { get; set; } = null!;
}