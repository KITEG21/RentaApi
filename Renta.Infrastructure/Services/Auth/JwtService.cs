using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Renta.Application.Settings;
using Renta.Application.Interfaces;

namespace Renta.Infrastructure.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(
        string userId, 
        string username, 
        string email, 
        string provider, 
        List<string>? roles = null,
        Guid? canonicalUserId = null)
    {
        var secret = _settings.Secret ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("provider", provider),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        
        if (roles != null && roles.Count > 0)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); 
                claims.Add(new Claim("role", role));
            }
        }

        if (canonicalUserId.HasValue)
        {
            claims.Add(new Claim("canonical_user_id", canonicalUserId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            signingCredentials: credentials
        );

        // Create handler without claim type mapping
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.OutboundClaimTypeMap.Clear();
        
        return tokenHandler.WriteToken(token);
    }
}