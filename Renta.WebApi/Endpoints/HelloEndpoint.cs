using System;

namespace Renta.WebApi.Endpoints;

public class HelloEndpoint : CoreEndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/hello");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync("Hello, World!", cancellation: ct);
    }
}
