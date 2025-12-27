using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Command.Put;

public class UpdateCarCommandHandler : CoreCommandHandler<UpdateCarCommand, UpdateCarResponse>
{
    public UpdateCarCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<UpdateCarResponse> ExecuteAsync(UpdateCarCommand command, CancellationToken ct = default)
    {
        var carRepo = UnitOfWork!.WriteDbRepository<Car>();
        var car = await carRepo.GetByIdAsync(command.Id);

        if (car is null)
            ThrowError($"Car with ID {command.Id} not found.", 404);

        car.Brand = command.Brand!;
        car.Model = command.Model!;
        car.Year = command.Year;
        car.Price = command.Price;
        car.Miles = command.Miles;
        car.Location = command.Location;
        car.Description = command.Description;
        car.MechanicalDetails = command.MechanicalDetails;
        car.PaymentConditions = command.PaymentConditions;
        car.History = command.History;
        await carRepo.UpdateAsync(car, true);

        return new UpdateCarResponse
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price
        };
    }
}