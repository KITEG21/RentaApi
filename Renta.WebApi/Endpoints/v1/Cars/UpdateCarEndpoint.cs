using System;
using FastEndpoints;
using Renta.Application.Features.Cars.Command.Put;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class UpdateCarEndpoint : CoreEndpoint<UpdateCarCommand, UpdateCarResponse>
{
    public override void Configure()
    {
        Put("/car/{Id}");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Updates an existing car")
            .WithDescription("Updates an existing car with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(UpdateCarCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}