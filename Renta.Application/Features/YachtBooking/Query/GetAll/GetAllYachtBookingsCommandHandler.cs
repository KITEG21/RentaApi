using Microsoft.EntityFrameworkCore;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.YachtBooking.Query.GetAll;

public class GetAllYachtBookingsCommandHandler : CoreQueryHandler<GetAllYachtBookingsCommand, PagedResponse<GetAllYachtBookingsResponse>>
{
    public GetAllYachtBookingsCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<PagedResponse<GetAllYachtBookingsResponse>> ExecuteAsync(GetAllYachtBookingsCommand command, CancellationToken ct = default)
    {
        var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var bookingsQuery = bookingRepo.GetAllFiltered(req: command.queryRequest);

        // If not admin/dealer, only show user's own bookings
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            var clientId = CurrentUserId;
            if (clientId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.ClientId == clientId.Value);
            }
        }

        var response = await bookingsQuery
            .Include(b => b.Yacht)
            .Include(b => b.Client)
            .Select(booking => new GetAllYachtBookingsResponse
            {
                Id = booking.Id,
                YachtId = booking.YachtId,
                YachtName = booking.Yacht.Name,
                ClientId = booking.ClientId,
                ClientName = booking.Client.FirstName + " " + booking.Client.LastName,
                Date = booking.Date,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalPrice,
                PaymentStatus = booking.PaymentStatus.ToString(),
                BookingStatus = booking.BookingStatus.ToString(),
                Created = booking.Created
            })
            .ToPagedResultAsync(command.queryRequest.Page, command.queryRequest.PerPage);

        return response;
    }
}
