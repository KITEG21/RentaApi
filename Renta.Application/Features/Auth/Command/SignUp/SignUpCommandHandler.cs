using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Renta.Application.Interfaces;
using Renta.Application.Settings;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;

namespace Renta.Application.Features.Auth.Command.SignUp;

public class SignUpCommandHandler : CoreCommandHandler<SignUpCommand, SignUpResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public SignUpCommandHandler(
        UserManager<User> userManager,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public override async Task<SignUpResponse> ExecuteAsync(SignUpCommand command, CancellationToken ct = default)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists");
        }

        existingUser = await _userManager.FindByNameAsync(command.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this username already exists");
        }

        // Create new user
        var user = new User
        {
            UserName = command.Username,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            UserType = command.UserType,
            EmailConfirmed = false, // TODO: Implement email confirmation
            Created = DateTime.UtcNow,
            StatusBaseEntity = StatusEntityType.Active
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(
            userId: user.Id.ToString(),
            username: user.UserName!,
            email: user.Email!,
            provider: "local",
            canonicalUserId: user.Id
        );

        return new SignUpResponse
        {
            UserId = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes)
        };
    }
}
