using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Renta.Application.Settings;

namespace Renta.WebApi.ServicesExtensions;

public static class JwtAuthenticationSetup
{
    public static IServiceCollection AddJwtAuthenticationSetup(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
        {
            throw new InvalidOperationException("JWT settings are not properly configured in appsettings.json");
        }

        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; // Set to true in production
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero, // Remove default 5 minute tolerance
                
                // IMPORTANT: Prevent claim type mapping
                // This keeps claim names as-is instead of converting to Microsoft claim types
                NameClaimType = "unique_name",
                RoleClaimType = "role"
            };

            // Optional: Add event handlers for debugging
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception}");
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine($"OnChallenge: {context.Error}, {context.ErrorDescription}");
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "Unauthorized",
                        message = "You are not authorized to access this resource"
                    });
                    return context.Response.WriteAsync(result);
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully!");
                    Console.WriteLine($"User Identity: {context.Principal?.Identity?.Name}");
                    Console.WriteLine($"Claims: {string.Join(", ", context.Principal?.Claims.Select(c => $"{c.Type}={c.Value}") ?? Array.Empty<string>())}");
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault();
                    Console.WriteLine($"Token received: {(string.IsNullOrEmpty(token) ? "NO TOKEN" : "TOKEN PRESENT")}");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}