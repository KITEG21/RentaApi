namespace Renta.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(
        string userId, 
        string username,
        string email, 
        string provider, 
        List<string>? roles = null,
        Guid? canonicalUserId = null);
}
