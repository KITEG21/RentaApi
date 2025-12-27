using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Query.GetById;

public class GetCarByIdCommandHandler : CoreQueryHandler<GetCarByIdCommand, GetCarByIdResponse>
{
    public GetCarByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetCarByIdResponse> ExecuteAsync(GetCarByIdCommand command, CancellationToken ct = default)
    {
        var carRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Car>();
        var car = carRepo.GetById(command.Id);

        if (car is null)
            ThrowError($"Car with ID {command.Id} not found.", 404);

        return new GetCarByIdResponse
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            Miles = car.Miles,
            Location = car.Location,
            Description = car.Description,
            MechanicalDetails = car.MechanicalDetails,
            PaymentConditions = car.PaymentConditions,
            History = car.History,
            Status = car.Status,
            DealerId = car.DealerId
        };
    }
}