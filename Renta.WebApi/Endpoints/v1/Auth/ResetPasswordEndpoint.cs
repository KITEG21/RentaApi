using FastEndpoints;
using Renta.Application.Features.Auth.Command.ResetPassword;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Auth;

public class ResetPasswordEndpoint : CoreEndpoint<ResetPasswordCommand, ResetPasswordResponse>
{
    public override void Configure()
    {
        Post("/auth/reset-password");
        Description(b => b
            .WithTags(RouteGroup.Auth)
            .WithSummary("Reset Password")
            .WithDescription("Reset user password with token received via email")
        );
        base.Configure();
    }

    public override async Task HandleAsync(ResetPasswordCommand req, CancellationToken ct)
    {
        var result = await req.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}