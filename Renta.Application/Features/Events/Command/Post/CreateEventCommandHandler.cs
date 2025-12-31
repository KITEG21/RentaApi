using System;
using Renta.Domain.Entities.Events;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Events.Command.Post;

public class CreateEventCommandHandler : CoreCommandHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly ILogger _logger;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<CreateEventCommandHandler>();
    }

    public override async Task<CreateEventResponse> ExecuteAsync(CreateEventCommand command, CancellationToken ct = default)
    {
        _logger.Information("Creating new event: {Title}", command.Title);

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

        _logger.Information("Successfully created event: {EventId}", newEvent.Id);

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
