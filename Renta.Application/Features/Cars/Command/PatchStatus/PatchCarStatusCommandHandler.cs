using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Command.Patch;

public class PatchCarStatusCommandHandler : CoreCommandHandler<PatchCarStatusCommand, PatchCarStatusResponse>
{
    public PatchCarStatusCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PatchCarStatusResponse> ExecuteAsync(PatchCarStatusCommand command, CancellationToken ct = default)
    {
        var carRepo = UnitOfWork!.WriteDbRepository<Car>();
        var car = await carRepo.GetByIdAsync(command.Id);

        if (car == null)
            throw new Exception($"Car with ID {command.Id} not found.");

        car.Status = command.Status;

        await carRepo.UpdateAsync(car, true);

        return new PatchCarStatusResponse
        {
            Id = car.Id,
            Status = car.Status
        };
    }
}