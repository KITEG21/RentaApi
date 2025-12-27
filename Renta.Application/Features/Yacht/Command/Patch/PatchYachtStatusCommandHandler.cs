using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yacht.Command.Patch;

public class PatchYachtStatusCommandHandler : CoreCommandHandler<PatchYachtStatusCommand, PatchYachtStatusResponse>
{
    public PatchYachtStatusCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PatchYachtStatusResponse> ExecuteAsync(PatchYachtStatusCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = await yachtRepo.GetByIdAsync(command.Id);

        if (yacht is null)
            ThrowError($"Yacht with ID {command.Id} not found.", 404);

        yacht.AvailabilityStatus = command.AvailabilityStatus;

        await yachtRepo.UpdateAsync(yacht, true);

        return new PatchYachtStatusResponse
        {
            Id = yacht.Id,
            AvailabilityStatus = yacht.AvailabilityStatus
        };
    }
}
