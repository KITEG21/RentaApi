using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Renta.Application.Interfaces;
using Renta.Application.Settings;
using Renta.Domain.Entities.Identity;

namespace Renta.Application.Features.Auth.Command.Login;

public class LoginCommandHandler : CoreCommandHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(
        UserManager<User> userManager,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public override async Task<LoginResponse> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(command.Username) || string.IsNullOrEmpty(command.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        // Find user by username or email
        var user = await _userManager.FindByNameAsync(command.Username)
                   ?? await _userManager.FindByEmailAsync(command.Username);

        if (user == null)
            ThrowError("Invalid credentials", 401);

        // Check password
        if (!await _userManager.CheckPasswordAsync(user, command.Password))
            ThrowError("Invalid credentials", 401);


        var token = _jwtService.GenerateToken(
            userId: user.Id.ToString(),
            username: user.UserName!,
            email: user.Email!,
            provider: "local"
        );

        return new LoginResponse
        {
            Token = token,
            Username = user.UserName!,
            Email = user.Email!,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes)
        };
    }
}