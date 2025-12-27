using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yachts.Command.Post;

public class CreateYachtCommandHandler : CoreCommandHandler<CreateYachtCommand, CreateYachtResponse>
{
    public CreateYachtCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    
    public override async Task<CreateYachtResponse> ExecuteAsync(CreateYachtCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.WriteDbRepository<Domain.Entities.Vehicles.Yacht>();

        var yacht = new Domain.Entities.Vehicles.Yacht
        {
            Name = command.Name!,
            SizeFt = command.SizeFt,
            Capacity = command.Capacity,
            Description = command.Description,
            IncludedServices = command.IncludedServices,
            PricePerHour = command.PricePerHour,
            PricePerDay = command.PricePerDay,
            AvailableRoutes = command.AvailableRoutes,
            OwnerId = Guid.Parse("6390a56a-f646-44d0-87d4-efa0199e5ef7") // ToDo: Set OwnerId when authentication is implemented
        };

        await yachtRepo.SaveAsync(yacht, true);

        return new CreateYachtResponse
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