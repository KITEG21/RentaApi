using System;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Renta.WebApi.ServicesExtensions;

public static class WriteDbContextSetup
{
    public static IServiceCollection AddWriteDbContextSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("WriteDefaultConnection"),
                    builder => { builder.MigrationsAssembly(typeof(ApplicationWriteDbContext).Assembly.FullName); }));
        }

        return services;
    }
}
