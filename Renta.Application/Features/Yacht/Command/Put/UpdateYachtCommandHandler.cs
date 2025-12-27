using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yacht.Command.Put;

public class UpdateYachtCommandHandler : CoreCommandHandler<UpdateYachtCommand, UpdateYachtResponse>
{
    public UpdateYachtCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<UpdateYachtResponse> ExecuteAsync(UpdateYachtCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = await yachtRepo.GetByIdAsync(command.Id);

        if (yacht is null)
            ThrowError($"Yacht with ID {command.Id} not found.", 404);

        yacht.Name = command.Name!;
        yacht.SizeFt = command.SizeFt;
        yacht.Capacity = command.Capacity;
        yacht.Description = command.Description;
        yacht.IncludedServices = command.IncludedServices;
        yacht.PricePerHour = command.PricePerHour;
        yacht.PricePerDay = command.PricePerDay;
        yacht.AvailableRoutes = command.AvailableRoutes;

        await yachtRepo.UpdateAsync(yacht, true);

        return new UpdateYachtResponse
        {
            Id = yacht.Id,
            Name = yacht.Name,
            SizeFt = yacht.SizeFt,
            Capacity = yacht.Capacity,
            PricePerHour = yacht.PricePerHour,
            PricePerDay = yacht.PricePerDay
        };
    }
}
