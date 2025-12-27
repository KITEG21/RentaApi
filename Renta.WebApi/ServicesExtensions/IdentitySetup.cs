using System;
using System.Security.Claims;
using Renta.Domain.Entities.Identity;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace Renta.WebApi.ServicesExtensions;

public static class IdentitySetup
{
    public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            })
            .AddEntityFrameworkStores<ApplicationWriteDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
