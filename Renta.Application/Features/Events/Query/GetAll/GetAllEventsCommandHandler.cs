using System;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Events.Query.GetAll;

public class GetAllEventsCommandHandler : CoreQueryHandler<GetAllEventsCommand, PagedResponse<GetAllEventsResponse>>
{
    private readonly ILogger _logger;

    public GetAllEventsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<GetAllEventsCommandHandler>();
    }

    public override async Task<PagedResponse<GetAllEventsResponse>> ExecuteAsync(GetAllEventsCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving all events with query parameters: Page {Page}, PerPage {PerPage}", command.queryRequest.Page, command.queryRequest.PerPage);

        var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
        var events = eventRepo.GetAllFiltered(req: command.queryRequest);

        var response = await events.ToPagedResultAsync(command.queryRequest.Page ?? 1, command.queryRequest.PerPage ?? 10, evt => new GetAllEventsResponse
            {
                Id = evt.Id,
                Title = evt.Title,
                EventType = evt.EventType,
                EventDate = evt.EventDate,
                Location = evt.Location,
                TotalCapacity = evt.TotalCapacity,
                AvailableTickets = evt.AvailableTickets,
                Status = evt.Status
            });

        _logger.Information("Successfully retrieved events");

        return response;
    }
}
