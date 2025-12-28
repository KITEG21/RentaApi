using FastEndpoints;

namespace Renta.Application.Features.Auth.Command.ForgotPassword;

public record ForgotPasswordCommand : ICommand<ForgotPasswordResponse>
{
    public string Email { get; set; } = string.Empty;
}