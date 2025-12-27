using System;
using FastEndpoints.Swagger;

namespace Renta.WebApi.ServicesExtensions;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
    {
        services.SwaggerDocument(o =>
        {
            o.MinEndpointVersion = 1;
            o.MaxEndpointVersion = 1;
            o.AutoTagPathSegmentIndex = 0;

            o.DocumentSettings = s =>
            {
                s.Title = "Renta API";
                s.Version = "v1";
            };
        });

        return services;
    }
}
