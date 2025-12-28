using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Identity;

namespace Renta.Application.Features.Auth.Command.ResetPassword;

public class ResetPasswordCommandHandler : CoreCommandHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager, IActiveUserSession activeUserSession) : base(activeUserSession)
    {
        _userManager = userManager;
    }

    public override async Task<ResetPasswordResponse> ExecuteAsync(ResetPasswordCommand command, CancellationToken ct = default)
    {
        var email = CurrentUserEmail ?? command.Email;

        if (string.IsNullOrWhiteSpace(email))
            ThrowError("Email is required", 400);

        // if (string.IsNullOrWhiteSpace(command.Token))
        //     ThrowError("Reset token is required", 400);

        if (string.IsNullOrWhiteSpace(command.NewPassword))
            ThrowError("New password is required", 400);

        if (command.NewPassword != command.ConfirmPassword)
            ThrowError("Passwords do not match", 400);

        // Find user
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            ThrowError("Invalid request", 400);

        // Reset password
        var result = await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            ThrowError($"Failed to reset password: {errors}", 400);
        }

        return new ResetPasswordResponse
        {
            Success = true,
            Message = "Password has been reset successfully. You can now login with your new password."
        };
    }
}