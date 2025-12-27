using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Command.PatchBaseStatusEntity;

public class PatchEventBaseStatusEntityCommandHandler : CoreCommandHandler<PatchEventBaseStatusEntityCommand, PatchEventBaseStatusEntityResponse>
{
    public PatchEventBaseStatusEntityCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
    public override async Task<PatchEventBaseStatusEntityResponse> ExecuteAsync(PatchEventBaseStatusEntityCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Events.Event>();
        var evt = await eventRepo.GetByIdAsync(command.Id);
        if (evt is null)
            ThrowError($"Event with ID {command.Id} not found.", 404);

        if (Enum.TryParse<Domain.Enums.StatusEntityType>(command.BaseStatus, out var status) == false)
            ThrowError($"Status value '{command.BaseStatus}' is invalid.", 400);

        evt.StatusBaseEntity = status;
        await eventRepo.UpdateAsync(evt, true);
        return new PatchEventBaseStatusEntityResponse
        {
            Id = evt.Id,
            BaseStatus = evt.StatusBaseEntity.ToString()
        };
    }
}
