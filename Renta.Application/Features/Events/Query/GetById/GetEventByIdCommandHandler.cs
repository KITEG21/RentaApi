using System;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Events.Query.GetById;

public class GetEventByIdCommandHandler : CoreQueryHandler<GetEventByIdCommand, GetEventByIdResponse>
{
    public GetEventByIdCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<GetEventByIdResponse> ExecuteAsync(GetEventByIdCommand command, CancellationToken ct = default)
    {
        var eventRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Events.Event>();
        var evt = eventRepo.GetById(command.Id);

        if (evt is null)
            ThrowError($"Event with ID {command.Id} not found.", 404);

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
