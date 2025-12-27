using System;
using Renta.Domain.Entities.Vehicles;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Cars.Command.Post;

public class CreateCarCommandHandler : CoreCommandHandler<CreateCarCommand, CreateCarResponse>
{
    public CreateCarCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    public override async Task<CreateCarResponse> ExecuteAsync(CreateCarCommand command, CancellationToken ct = default)
    {
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
            DealerId = Guid.Parse("6390a56a-f646-44d0-87d4-efa0199e5ef7") // ToDo: Set DealerId when authentication is implemented
            
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
