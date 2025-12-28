using FastEndpoints;
using Renta.Application.Features.Auth.Command.ForgotPassword;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Auth;

public class ForgotPasswordEndpoint : CoreEndpoint<ForgotPasswordCommand, ForgotPasswordResponse>
{
    public override void Configure()
    {
        Post("/auth/forgot-password");
        AllowAnonymous();
        Description(b => b
            .WithTags(RouteGroup.Auth)
            .WithSummary("Forgot Password")
            .WithDescription("Send password reset link to user's email")
        );
        base.Configure();
    }

    public override async Task HandleAsync(ForgotPasswordCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}