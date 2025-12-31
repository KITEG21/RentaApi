using System;
using Renta.Application.Common.Response;
using Renta.Application.Extensions;
using Renta.Application.Features.Yacht.Query.GetAll;
using Renta.Domain.Interfaces.Repositories;
using Serilog;

namespace Renta.Application.Features.Yachts.Query.GetAll;

public class GetAllYachtsCommandHandler : CoreQueryHandler<GetAllYachtsCommand, PagedResponse<GetAllYachtsResponse>>
{
    private readonly ILogger _logger;

    public GetAllYachtsCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _logger = Log.ForContext<GetAllYachtsCommandHandler>();
    }

    public override async Task<PagedResponse<GetAllYachtsResponse>> ExecuteAsync(GetAllYachtsCommand command, CancellationToken ct = default)
    {
        _logger.Information("Retrieving all yachts with query parameters: Page {Page}, PerPage {PerPage}", command.queryRequest.Page, command.queryRequest.PerPage);

        var yachtRepo = UnitOfWork!.ReadDbRepository<Domain.Entities.Vehicles.Yacht>();
        var yachts = yachtRepo.GetAllFiltered(req: command.queryRequest);

        var response = await yachts.ToPagedResultAsync(command.queryRequest.Page ?? 1, command.queryRequest.PerPage ?? 10, yacht => new GetAllYachtsResponse
            {
                Id = yacht.Id,
                Name = yacht.Name,
                SizeFt = yacht.SizeFt,
                Capacity = yacht.Capacity,
                PricePerHour = yacht.PricePerHour,
                PricePerDay = yacht.PricePerDay
            });

        _logger.Information("Successfully retrieved yachts");

        return response;
    }
}