using Scalar.AspNetCore;

namespace Renta.WebApi.ServicesExtensions;

public static class ScalarSetup
{
    public static IEndpointRouteBuilder UseScalarSetup(this IEndpointRouteBuilder app)
    {
        var env = app.ServiceProvider.GetService<IWebHostEnvironment>();
        if (env?.IsDevelopment() ?? false)
        {
            app.MapScalarApiReference(options =>
            {
                options.Title = "Renta API";
                options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                options.DarkMode = false;
                options.Theme = ScalarTheme.Default;
            });
        }

        return app;
    }
}