using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using YachtCalendarEntity = Renta.Domain.Entities.Bookings.YachtCalendar;

namespace Renta.Application.Features.YachtCalendar.Command.Delete;

public class DeleteYachtCalendarCommandHandler : CoreCommandHandler<DeleteYachtCalendarCommand, DeleteYachtCalendarResponse>
{
    public DeleteYachtCalendarCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    public override async Task<DeleteYachtCalendarResponse> ExecuteAsync(DeleteYachtCalendarCommand command, CancellationToken ct = default)
    {
        // Only Admin, Dealer can delete calendar entries
        if (!UserRoles.Contains("Admin") && !UserRoles.Contains("Dealer"))
        {
            ThrowError("You don't have permission to delete calendar entries.", 403);
        }

        var calendarReadRepo = UnitOfWork!.ReadDbRepository<YachtCalendarEntity>();
        var entry = calendarReadRepo.GetById(command.Id);

        if (entry is null)
        {
            ThrowError($"Calendar entry with ID {command.Id} not found.", 404);
        }

        var calendarWriteRepo = UnitOfWork!.WriteDbRepository<YachtCalendarEntity>();
        calendarWriteRepo.Delete(entry);
        await UnitOfWork!.SaveChangesAsync();

        return new DeleteYachtCalendarResponse
        {
            Id = command.Id,
            Message = "Calendar entry deleted successfully"
        };
    }
}
