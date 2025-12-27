using System;
using FastEndpoints;
using Renta.Application.Features.Cars.Query.GetById;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class GetCarByIdEndpoint : CoreEndpoint<GetCarByIdCommand, GetCarByIdResponse>
{
    public override void Configure()
    {
        Get("/car/{Id}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Retrieves a car by ID")
            .WithDescription("Retrieves detailed information about a specific car by its unique identifier.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(GetCarByIdCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}