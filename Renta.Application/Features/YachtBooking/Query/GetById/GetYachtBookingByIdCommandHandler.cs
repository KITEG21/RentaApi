using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.YachtBooking.Query.GetById;

public class GetYachtBookingByIdCommandHandler : CoreQueryHandler<GetYachtBookingByIdCommand, GetYachtBookingByIdResponse>
{
    private readonly ILogger _logger;

    public GetYachtBookingByIdCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
        _logger = Log.ForContext<GetYachtBookingByIdCommandHandler>();
    }

    public override async Task<GetYachtBookingByIdResponse> ExecuteAsync(GetYachtBookingByIdCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving yacht booking by ID: {BookingId} for user: {UserId}", command.Id, CurrentUserId);

        var bookingRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Bookings.YachtBooking>();
        var booking = await bookingRepo
            .GetAll()
            .Include(b => b.Yacht)
            .Include(b => b.Client)
            .FirstOrDefaultAsync(b => b.Id == command.Id, ct);

        if (booking is null)
        {
            _logger.Warning("Booking not found: {BookingId}", command.Id);
            ThrowError($"Booking with ID {command.Id} not found.", 404);
        }

        // Only allow user to see their own bookings (unless admin/dealer)
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            var clientId = CurrentUserId;
            if (booking.ClientId != clientId)
            {
                _logger.Warning("Access denied: User {UserId} attempted to access booking {BookingId} owned by {OwnerId}", CurrentUserId, command.Id, booking.ClientId);
                ThrowError("You don't have permission to view this booking.", 403);
            }
        }

        _logger.Information("Successfully retrieved booking: {BookingId}", command.Id);

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
