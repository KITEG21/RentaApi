using Microsoft.EntityFrameworkCore;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.YachtBooking.Query.GetAll;

public class GetAllYachtBookingsCommandHandler : CoreQueryHandler<GetAllYachtBookingsCommand, PagedResponse<GetAllYachtBookingsResponse>>
{
    private readonly ILogger _logger;

    public GetAllYachtBookingsCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
        _logger = Log.ForContext<GetAllYachtBookingsCommandHandler>();
    }

    public override async Task<PagedResponse<GetAllYachtBookingsResponse>> ExecuteAsync(GetAllYachtBookingsCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving all yacht bookings with query parameters: Page {Page}, PerPage {PerPage} for user: {UserId}", command.queryRequest.Page, command.queryRequest.PerPage, CurrentUserId);

        var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var bookingsQuery = bookingRepo.GetAllFiltered(req: command.queryRequest);

        // If not admin/dealer, only show user's own bookings
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            var clientId = CurrentUserId;
            if (clientId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.ClientId == clientId.Value);
                _logger.Information("Filtering bookings for client: {ClientId}", clientId);
            }
        }

        var response = await bookingsQuery
            .Include(b => b.Yacht)
            .Include(b => b.Client)
            .ToPagedResultAsync(command.queryRequest.Page ?? 1, command.queryRequest.PerPage ?? 10, booking => new GetAllYachtBookingsResponse
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
            });

        _logger.Information("Successfully retrieved yacht bookings");

        return response;
    }
}
