using System;
using FastEndpoints;
using Renta.Application.Settings;

namespace Renta.WebApi.ServicesExtensions;

public static class FastEndpointSetup
{
    public static IServiceCollection AddFastEndpointSetup(this IServiceCollection services)
    {
        services.AddFastEndpoints();

        return services;
    }

    public static IApplicationBuilder UseFastEndpointsSetup(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(c =>
        {
            c.Security.RoleClaimType = System.Security.Claims.ClaimTypes.Role;
            
            // c.Errors.UseProblemDetails();
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
            {
                return new
                {
                    StatusCode = statusCode,
                    Errors = failures.Select(f => new
                    {
                        name = f.PropertyName,
                        code = f.ErrorCode,
                        // reason = f.ErrorMessage,
                        reason = TryParseJson(f.ErrorMessage) ? null : f.ErrorMessage,
                        variables = TryParseJson(f.ErrorMessage) ? f.ErrorMessage : null
                    })
                };
            };
            c.Versioning.Prefix = "v";
            c.Versioning.PrependToRoute = true;
            c.Versioning.DefaultVersion = 1;

            c.Serializer.Options.Converters.Add(new DateTimeConverterWithoutZ());
        });

        return app;
    }

    private static bool TryParseJson(string errorMessage)
    {
        try
        {
            System.Text.Json.JsonSerializer.Deserialize<object>(errorMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }
}