using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.YachtBooking.Query.GetById;

public class GetYachtBookingByIdCommandHandler : CoreQueryHandler<GetYachtBookingByIdCommand, GetYachtBookingByIdResponse>
{
    public GetYachtBookingByIdCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<GetYachtBookingByIdResponse> ExecuteAsync(GetYachtBookingByIdCommand command, CancellationToken ct = default)
    {
        var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var booking = await bookingRepo
            .GetAll()
            .Include(b => b.Yacht)
            .Include(b => b.Client)
            .FirstOrDefaultAsync(b => b.Id == command.Id, ct);

        if (booking is null)
        {
            ThrowError($"Booking with ID {command.Id} not found.", 404);
        }

        // Only allow user to see their own bookings (unless admin/dealer)
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            var clientId = CurrentUserId;
            if (booking.ClientId != clientId)
            {
                ThrowError("You don't have permission to view this booking.", 403);
            }
        }

        return new GetYachtBookingByIdResponse
        {
            Id = booking.Id,
            YachtId = booking.YachtId,
            YachtName = booking.Yacht.Name,
            YachtSizeFt = booking.Yacht.SizeFt,
            YachtCapacity = booking.Yacht.Capacity,
            ClientId = booking.ClientId,
            ClientName = booking.Client.FirstName + " " + booking.Client.LastName,
            ClientEmail = booking.Client.Email ?? string.Empty,
            Date = booking.Date,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            AdditionalServices = booking.AdditionalServices,
            TotalPrice = booking.TotalPrice,
            PaymentStatus = booking.PaymentStatus.ToString(),
            BookingStatus = booking.BookingStatus.ToString(),
            Created = booking.Created,
            LastModified = booking.LastModified
        };
    }
}
