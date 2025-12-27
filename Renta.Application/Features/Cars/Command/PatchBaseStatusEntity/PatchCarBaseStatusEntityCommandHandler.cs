using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Command.PatchBaseStatusEntity;

public class PatchCarBaseStatusEntityCommandHandler : CoreCommandHandler<PatchCarBaseStatusEntityCommand, PatchCarBaseStatusEntityResponse>
{
    public PatchCarBaseStatusEntityCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {

    }
    public override async Task<PatchCarBaseStatusEntityResponse> ExecuteAsync(PatchCarBaseStatusEntityCommand command, CancellationToken ct = default)
    {
        var carRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Vehicles.Car>();
        var car = await carRepo.GetByIdAsync(command.Id);
        if (car is null)
            ThrowError($"Car with ID {command.Id} not found.", 404);

        if (Enum.TryParse<Domain.Enums.StatusEntityType>(command.BaseStatus, out var status) == false)
            ThrowError($"Status value '{command.BaseStatus}' is invalid.", 400);

        car.StatusBaseEntity = status;
        await carRepo.UpdateAsync(car, true);
        return new PatchCarBaseStatusEntityResponse
        {
            Id = car.Id,
            BaseStatus = car.StatusBaseEntity.ToString()
        };
    }
}
