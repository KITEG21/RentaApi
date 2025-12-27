using FastEndpoints;
using Renta.Application.Features.Auth.Command.SignUp;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Auth;

public class SignUpEndpoint : CoreEndpoint<SignUpCommand, SignUpResponse>
{
    public override void Configure()
    {
        Post("/auth/signup");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Auth)
            .WithSummary("Sign up endpoint")
            .WithDescription("Register a new user account and return JWT token")
        );
        base.Configure();
    }

    public override async Task HandleAsync(SignUpCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.CreatedAtAsync<GetProfileEndpoint>(new { }, result, cancellation: ct);
    }
}
