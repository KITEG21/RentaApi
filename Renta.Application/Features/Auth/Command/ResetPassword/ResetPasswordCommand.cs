using FastEndpoints;

namespace Renta.Application.Features.Auth.Command.ResetPassword;

public record ResetPasswordCommand : ICommand<ResetPasswordResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}