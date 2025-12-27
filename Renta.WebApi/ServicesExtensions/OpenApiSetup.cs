using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Renta.WebApi.ServicesExtensions;

public static class OpenApiSetup
{
    public static IServiceCollection AddOpenApiSetup(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "Renta API";
                document.Info.Version = "v1";
                document.Info.Description = "API for Renta application with JWT authentication";

                // Add JWT Bearer security scheme
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token",
                    In = ParameterLocation.Header,
                    Name = "Authorization"
                });

                // Apply security requirement globally
                document.SecurityRequirements = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }] = Array.Empty<string>()
                    }
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }
}