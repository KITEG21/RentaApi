namespace Renta.Application.Features.Auth.Command.SignUp;

public record SignUpResponse
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}
