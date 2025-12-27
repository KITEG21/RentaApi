using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Command.Patch;

public class PatchEventStatusCommandHandler : CoreCommandHandler<PatchEventStatusCommand, PatchEventStatusResponse>
{
    public PatchEventStatusCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PatchEventStatusResponse> ExecuteAsync(PatchEventStatusCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.WriteDbRepository<Event>();
        var evt = await eventRepo.GetByIdAsync(command.Id);

        if (evt is null)
            ThrowError($"Event with ID {command.Id} not found.", 404);

        evt.Status = command.Status;

        await eventRepo.UpdateAsync(evt, true);

        return new PatchEventStatusResponse
        {
            Id = evt.Id,
            Status = evt.Status
        };
    }
}
