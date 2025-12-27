using System;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Query.GetAll;

public class GetAllEventsCommandHandler : CoreQueryHandler<GetAllEventsCommand, PagedResponse<GetAllEventsResponse>>
{
    public GetAllEventsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PagedResponse<GetAllEventsResponse>> ExecuteAsync(GetAllEventsCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
        var events = eventRepo.GetAllFiltered(req: command.queryRequest);

        var response = await events.Select(evt => new GetAllEventsResponse
            {
                Id = evt.Id,
                Title = evt.Title,
                EventType = evt.EventType,
                EventDate = evt.EventDate,
                Location = evt.Location,
                TotalCapacity = evt.TotalCapacity,
                AvailableTickets = evt.AvailableTickets,
                Status = evt.Status
            }).ToPagedResultAsync(command.queryRequest.Page, command.queryRequest.PerPage);

        return response;
    }
}
