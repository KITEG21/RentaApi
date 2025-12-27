using System;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Services;
using Renta.Domain.Settings;
using Renta.Infrastructure.Concrete;
using Renta.Infrastructure.Persistence.Repositories;
using Renta.Infrastructure.Services;
using Microsoft.Extensions.Options;

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
        // Configure named HttpClients with pre-configured settings
        
        
        services.AddHttpClient();



        services.Configure<ThrottleSettings>(configuration.GetSection("Throttle"));
        return services;
    }
}
