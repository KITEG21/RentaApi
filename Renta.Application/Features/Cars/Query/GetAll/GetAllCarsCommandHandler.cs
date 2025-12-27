using System;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Query.GetAll;

public class GetAllCarsCommandHandler : CoreQueryHandler<GetAllCarsCommand, PagedResponse<GetAllCarsResponse>>
{
    public GetAllCarsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PagedResponse<GetAllCarsResponse>> ExecuteAsync(GetAllCarsCommand command, CancellationToken ct = default)
    {
        var carRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Car>();
        var cars = carRepo.GetAllFiltered(req: command.queryRequest);

        var response = await cars.Select(car => new GetAllCarsResponse
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price
            }).ToPagedResultAsync(command.queryRequest.Page, command.queryRequest.PerPage);

        return response;
    }
}
