using System;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Command.Post;

public class CreateCarCommandHandler : CoreCommandHandler<CreateCarCommand, CreateCarResponse>
{
    public CreateCarCommandHandler(IUnitOfWork unitOfWork, IActiveUserSession activeUserSession) : base(activeUserSession, unitOfWork)
    {
    }
    public override async Task<CreateCarResponse> ExecuteAsync(CreateCarCommand command, CancellationToken ct = default)
    {
        var userId = CurrentUserId;
        var carRepo = UnitOfWork!.WriteDbRepository<Car>();

        var car = new Car
        {
            Brand = command.Brand!,
            Model = command.Model!,
            Year = command.Year,
            Price = command.Price,
            Miles = command.Miles,
            Location = command.Location,
            Description = command.Description,
            MechanicalDetails = command.MechanicalDetails,
            PaymentConditions = command.PaymentConditions,
            DealerId = userId!.Value
            
        };

        await carRepo.SaveAsync(car, true);

        return new CreateCarResponse
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price
        };

    }
}
