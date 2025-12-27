using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Command.Post;

public class CreateEventCommandHandler : CoreCommandHandler<CreateEventCommand, CreateEventResponse>
{
    public CreateEventCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<CreateEventResponse> ExecuteAsync(CreateEventCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.WriteDbRepository<Event>();

        var newEvent = new Event
        {
            Title = command.Title!,
            Description = command.Description,
            EventType = command.EventType,
            EventDate = command.EventDate,
            Location = command.Location,
            GeneralTicketPrice = command.GeneralTicketPrice,
            VIPTicketPrice = command.VIPTicketPrice,
            BackstageTicketPrice = command.BackstageTicketPrice,
            TotalCapacity = command.TotalCapacity,
            AvailableTickets = command.TotalCapacity,
            GeneralBenefits = command.GeneralBenefits,
            VIPBenefits = command.VIPBenefits,
            BackstageBenefits = command.BackstageBenefits
        };

        await eventRepo.SaveAsync(newEvent, true);

        return new CreateEventResponse
        {
            Id = newEvent.Id,
            Title = newEvent.Title,
            EventDate = newEvent.EventDate,
            Location = newEvent.Location,
            TotalCapacity = newEvent.TotalCapacity
        };
    }
}
