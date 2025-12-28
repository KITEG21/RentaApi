using System;
using Renta.Application.Interfaces;
using Renta.Application.Settings;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Services;
using Renta.Domain.Settings;
using Renta.Infrastructure.Concrete;
using Renta.Infrastructure.Persistence.Repositories;
using Renta.Infrastructure.Services;
using Renta.Infrastructure.Services.Auth;
using Microsoft.Extensions.Options;
using Renta.Infrastructure.Services.Email;
using Renta.Infrastructure.Services.QRCode;

namespace Renta.WebApi.ServicesExtensions;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddDependencyInjectionSetup(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // HttpContextAccessor is required by ActiveUserSession to access HttpContext.User
        services.AddHttpContextAccessor();
        services.AddScoped<IActiveUserSession, ActiveUserSession>();
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped(typeof(IReadGenericCoreRepository<>), typeof(ReadGenericCoreRepository<>));
        services.AddScoped(typeof(IWriteGenericCoreRepository<>), typeof(WriteGenericCoreRepository<>));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IQRCodeService, QRCodeService>();
        
        services.AddHttpClient();

        // JWT Service
        services.AddScoped<IJwtService, JwtService>();

        services.Configure<ThrottleSettings>(configuration.GetSection("Throttle"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        return services;
    }
}
