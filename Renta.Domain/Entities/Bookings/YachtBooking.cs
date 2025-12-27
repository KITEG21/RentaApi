using System;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Bookings;

public class YachtBooking: Entity
{
    public Guid YachtId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string AdditionalServices { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;
    public Yacht Yacht { get; set; } = null!;
    public User Client { get; set; } = null!;
}
