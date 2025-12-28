using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Interfaces.Services;

namespace Renta.Application.Features.Auth.Command.ForgotPassword;

public class ForgotPasswordCommandHandler : CoreCommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public override async Task<ForgotPasswordResponse> ExecuteAsync(ForgotPasswordCommand command, CancellationToken ct = default)
    {


        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            return new ForgotPasswordResponse
            {
                Message = "We have sent a email to your email address to reset your password."
            };
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        try
        {
            await _emailService.SendPasswordResetEmailAsync(user.Email!, resetToken, user.UserName!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }

        return new ForgotPasswordResponse
        {
            Message = "We have sent a email to your email address to reset your password."
        };
    }
}