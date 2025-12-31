using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Events.Command.Put;

public class UpdateEventCommandHandler : CoreCommandHandler<UpdateEventCommand, UpdateEventResponse>
{
    private readonly ILogger _logger;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<UpdateEventCommandHandler>();
    }

    public override async Task<UpdateEventResponse> ExecuteAsync(UpdateEventCommand command, CancellationToken ct = default)
    {
        _logger.Information("Updating event: {EventId}", command.Id);

        var eventRepo = UnitOfWork!.WriteDbRepository<Event>();
        var evt = await eventRepo.GetByIdAsync(command.Id);

        if (evt is null)
        {
            _logger.Warning("Event not found for update: {EventId}", command.Id);
            ThrowError($"Event with ID {command.Id} not found.", 404);
        }

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

        _logger.Information("Successfully updated event: {EventId}", command.Id);

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
