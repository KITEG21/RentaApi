using Renta.Infrastructure.Persistence.Context;
using Renta.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Renta.WebApi;

public static class ServiceExtensions
{

    public static void AddGenericRepositoryExtension(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(Renta.Domain.Interfaces.Repositories.IWriteGenericCoreRepository<>), typeof(WriteGenericCoreRepository<>));
    }

    public static void AddCorsExtension(this IServiceCollection services)
    {
        services.AddCors(cors =>
        {
            cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });
    }

    public static void AddHttpClientExtensions(this IServiceCollection services)
    {
        services.AddHttpClient("Contracting", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.example.com/");
        });
    }

    // public static IServiceCollection AddJWTAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
    // {
    //     services.AddAuthentication(options =>
    //         {
    //             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //         })
    //         .AddJwtBearer(options =>
    //         {
    //             options.TokenValidationParameters = new TokenValidationParameters
    //             {
    //                 ValidateIssuer = true,
    //                 ValidateAudience = true,
    //                 ValidateLifetime = true,
    //                 ValidateIssuerSigningKey = true,
    //                 ValidIssuer = configuration["TokenOptions:Issuer"],
    //                 ValidAudience = configuration["TokenOptions:Audience"],
    //                 IssuerSigningKey =
    //                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenOptions:SecurityKey"]))
    //             };
    //         });

    //     return services;
    // }

        public static void AddDbContextExtension(this IServiceCollection services, IConfiguration configuration)
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
    }


    public static IServiceProvider ApplyMigrationsExtension(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();

        var pendingMigration = db.Database.GetPendingMigrations();
        if (pendingMigration.Any())
        {
            db.Database.Migrate();
        }
        
        return serviceProvider;
    }
}
