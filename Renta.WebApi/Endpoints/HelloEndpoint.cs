using System;

namespace Renta.WebApi.Endpoints;

public class HelloEndpoint : CoreEndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/hello");
        AllowAnonymous();
        Description(b => b.WithTags("Hello"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync("Hello, World!", cancellation: ct);
    }
}
