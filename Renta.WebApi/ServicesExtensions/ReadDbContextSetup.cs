using System;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Renta.WebApi.ServicesExtensions;

public static class ReadDbContextSetup
{
    public static IServiceCollection AddReadDbContextSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationReadDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            services.AddDbContext<ApplicationReadDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("ReadDefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(ApplicationWriteDbContext).Assembly.FullName);
                        builder.CommandTimeout(120);
                    }));
        }

        return services;
    }
}
