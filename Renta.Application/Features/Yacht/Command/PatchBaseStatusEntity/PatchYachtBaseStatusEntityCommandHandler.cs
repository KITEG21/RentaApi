using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yacht.Command.PatchBaseStatusEntity;

public class PatchYachtBaseStatusEntityCommandHandler : CoreCommandHandler<PatchYachtBaseStatusEntityCommand, PatchYachtBaseStatusEntityResponse>
{
    public PatchYachtBaseStatusEntityCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
    public override async Task<PatchYachtBaseStatusEntityResponse> ExecuteAsync(PatchYachtBaseStatusEntityCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = await yachtRepo.GetByIdAsync(command.Id);
        if (yacht is null)
            ThrowError($"Yacht with ID {command.Id} not found.", 404);

        if (Enum.TryParse<Domain.Enums.StatusEntityType>(command.BaseStatus, out var status) == false)
            ThrowError($"Status value '{command.BaseStatus}' is invalid.", 400);

        yacht.StatusBaseEntity = status;
        await yachtRepo.UpdateAsync(yacht, true);
        return new PatchYachtBaseStatusEntityResponse
        {
            Id = yacht.Id,
            BaseStatus = yacht.StatusBaseEntity.ToString()
        };
    }
}
