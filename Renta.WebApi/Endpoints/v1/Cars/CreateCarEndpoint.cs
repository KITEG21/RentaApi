using System;
using FastEndpoints;
using Renta.Application.Features.Cars.Command.Post;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Cars;

public class CreateCarEndpoint : CoreEndpoint<CreateCarCommand, CreateCarResponse>
{
    public override void Configure()
    {
        Post("/car");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Cars)
            .WithSummary("Creates a new car")
            .WithDescription("Creates a new car with the provided details.")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CreateCarCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
