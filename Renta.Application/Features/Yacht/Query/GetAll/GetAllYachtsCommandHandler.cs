using System;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Features.Yacht.Query.GetAll;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Yachts.Query.GetAll;

public class GetAllYachtsCommandHandler : CoreQueryHandler<GetAllYachtsCommand, PagedResponse<GetAllYachtsResponse>>
{
    public GetAllYachtsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<PagedResponse<GetAllYachtsResponse>> ExecuteAsync(GetAllYachtsCommand command, CancellationToken ct = default)
    {
        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yachts = yachtRepo.GetAllFiltered(req: command.queryRequest);

        var response = await yachts.Select(yacht => new GetAllYachtsResponse
            {
                Id = yacht.Id,
                Name = yacht.Name,
                SizeFt = yacht.SizeFt,
                Capacity = yacht.Capacity,
                PricePerHour = yacht.PricePerHour,
                PricePerDay = yacht.PricePerDay
            }).ToPagedResultAsync(command.queryRequest.Page, command.queryRequest.PerPage);

        return response;
    }
}