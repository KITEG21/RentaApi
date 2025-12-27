using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yacht.Query.GetById;

public class GetYachtByIdCommandHandler : CoreQueryHandler<GetYachtByIdCommand, GetYachtByIdResponse>
{
    public GetYachtByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetYachtByIdResponse> ExecuteAsync(GetYachtByIdCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yacht = yachtRepo.GetById(command.Id);

        if (yacht is null)
            ThrowError($"Yacht with ID {command.Id} not found.", 404);

        return new GetYachtByIdResponse
        {
            Id = yacht.Id,
            Name = yacht.Name,
            SizeFt = yacht.SizeFt,
            Capacity = yacht.Capacity,
            Description = yacht.Description,
            IncludedServices = yacht.IncludedServices,
            PricePerHour = yacht.PricePerHour,
            PricePerDay = yacht.PricePerDay,
            AvailableRoutes = yacht.AvailableRoutes,
            AvailabilityStatus = yacht.AvailabilityStatus,
            OwnerId = yacht.OwnerId
        };
    }
}
