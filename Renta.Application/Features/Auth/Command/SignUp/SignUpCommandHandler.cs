
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Renta.Application.Interfaces;
using Renta.Application.Settings;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;
using Serilog;

namespace Renta.Application.Features.Auth.Command.SignUp;


public class SignUpCommandHandler : CoreCommandHandler<SignUpCommand, SignUpResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger _logger;

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
        _logger = Log.ForContext<SignUpCommandHandler>();
    }

    public override async Task<SignUpResponse> ExecuteAsync(SignUpCommand command, CancellationToken ct = default)
    {
        _logger.Information("SignUp attempt for email: {Email}, username: {Username}", command.Email, command.Username);

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser != null)
        {
            _logger.Warning("SignUp failed: Email already exists: {Email}", command.Email);
            ThrowError("A user with this email already exists");
        }

        existingUser = await _userManager.FindByNameAsync(command.Username);
        if (existingUser != null)
        {
            _logger.Warning("SignUp failed: Username already exists: {Username}", command.Username);
            ThrowError("A user with this username already exists");
        }

        // Validate role exists
        var roleExists = await _roleManager.RoleExistsAsync(command.Role);
        if (!roleExists)
        {
            _logger.Warning("SignUp failed: Role does not exist: {Role}", command.Role);
            ThrowError($"Role '{command.Role}' does not exist");
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
            _logger.Error("SignUp failed: Could not create user {Email}. Errors: {Errors}", command.Email, errors);
            ThrowError($"Failed to create user: {errors}");
        }

        // Assign role to user
        var roleResult = await _userManager.AddToRoleAsync(user, command.Role);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            _logger.Error("SignUp failed: Could not assign role {Role} to user {Email}. Errors: {Errors}", command.Role, command.Email, errors);
            ThrowError($"Failed to assign role: {errors}");
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

        _logger.Information("SignUp succeeded for user: {Email}", command.Email);

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
