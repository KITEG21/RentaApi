using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Command.Put;

public class UpdateEventCommandHandler : CoreCommandHandler<UpdateEventCommand, UpdateEventResponse>
{
    public UpdateEventCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<UpdateEventResponse> ExecuteAsync(UpdateEventCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.WriteDbRepository<Event>();
        var evt = await eventRepo.GetByIdAsync(command.Id);

        if (evt is null)
            ThrowError($"Event with ID {command.Id} not found.", 404);

        evt.Title = command.Title!;
        evt.Description = command.Description;
        evt.EventType = command.EventType;
        evt.EventDate = command.EventDate;
        evt.Location = command.Location;
        evt.GeneralTicketPrice = command.GeneralTicketPrice;
        evt.VIPTicketPrice = command.VIPTicketPrice;
        evt.BackstageTicketPrice = command.BackstageTicketPrice;
        evt.TotalCapacity = command.TotalCapacity;
        evt.GeneralBenefits = command.GeneralBenefits;
        evt.VIPBenefits = command.VIPBenefits;
        evt.BackstageBenefits = command.BackstageBenefits;

        await eventRepo.UpdateAsync(evt, true);

        return new UpdateEventResponse
        {
            Id = evt.Id,
            Title = evt.Title,
            EventDate = evt.EventDate,
            Location = evt.Location,
            TotalCapacity = evt.TotalCapacity
        };
    }
}
