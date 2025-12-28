namespace Renta.Application.Features.Auth.Command.ForgotPassword;

public record ForgotPasswordResponse
{
    public string Message { get; set; } = string.Empty;
}