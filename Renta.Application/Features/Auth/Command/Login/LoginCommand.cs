using FastEndpoints;

namespace Renta.Application.Features.Auth.Command.Login;

public record LoginCommand : ICommand<LoginResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
