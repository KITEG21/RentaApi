using FastEndpoints;
using Renta.Application.Features.Auth.Command.Login;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Auth;

public class LoginEndpoint : CoreEndpoint<LoginCommand, LoginResponse>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Auth)
            .WithSummary("Login endpoint")
            .WithDescription("Authenticate user and return JWT token")
        );
        base.Configure();
    }

    public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
    {
            var result = await req.ExecuteAsync(ct);
            await Send.OkAsync(result, ct);
    }
}
