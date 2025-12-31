using System;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Events.Query.GetById;

public class GetEventByIdCommandHandler : CoreQueryHandler<GetEventByIdCommand, GetEventByIdResponse>
{
    private readonly ILogger _logger;

    public GetEventByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<GetEventByIdCommandHandler>();
    }

    public override async Task<GetEventByIdResponse> ExecuteAsync(GetEventByIdCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving event by ID: {EventId}", command.Id);

        var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
        var evt = eventRepo.GetById(command.Id);

        if (evt is null)
        {
            _logger.Warning("Event not found: {EventId}", command.Id);
            ThrowError($"Event with ID {command.Id} not found.", 404);
        }

        _logger.Information("Successfully retrieved event: {EventId}", command.Id);

        return new GetEventByIdResponse
        {
            Id = evt.Id,
            Title = evt.Title,
            Description = evt.Description,
            EventType = evt.EventType,
            EventDate = evt.EventDate,
            Location = evt.Location,
            GeneralTicketPrice = evt.GeneralTicketPrice,
            VIPTicketPrice = evt.VIPTicketPrice,
            BackstageTicketPrice = evt.BackstageTicketPrice,
            TotalCapacity = evt.TotalCapacity,
            AvailableTickets = evt.AvailableTickets,
            GeneralBenefits = evt.GeneralBenefits,
            VIPBenefits = evt.VIPBenefits,
            BackstageBenefits = evt.BackstageBenefits,
            Status = evt.Status
        };
    }
}
