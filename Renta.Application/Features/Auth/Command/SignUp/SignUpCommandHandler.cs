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
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public SignUpCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

        // Validate role exists
        var roleExists = await _roleManager.RoleExistsAsync(command.Role);
        if (!roleExists)
        {
            throw new InvalidOperationException($"Role '{command.Role}' does not exist");
        }

        // Create new user
        var user = new User
        {
            UserName = command.Username,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            EmailConfirmed = false,
            Created = DateTime.UtcNow,
            StatusBaseEntity = StatusEntityType.Active
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        // Assign role to user
        var roleResult = await _userManager.AddToRoleAsync(user, command.Role);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to assign role: {errors}");
        }

        // Get roles for JWT
        var roles = await _userManager.GetRolesAsync(user);

        // Generate JWT token
        var token = _jwtService.GenerateToken(
            userId: user.Id.ToString(),
            username: user.UserName!,
            email: user.Email!,
            provider: "local",
            roles: roles.ToList(),
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
