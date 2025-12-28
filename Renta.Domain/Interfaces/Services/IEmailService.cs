using System.Threading.Tasks;

namespace Renta.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetToken, string username);
    Task SendEmailAsync(string toEmail, string subject, string body);
}