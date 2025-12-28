namespace Renta.Application.Features.Auth.Command.ResetPassword;

public record ResetPasswordResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
}