using System.IdentityModel.Tokens.Jwt;
using Microsoft.ApplicationInsights.Extensibility;
using Renta.WebApi;
using Renta.WebApi.Authorization;
using Renta.WebApi.ServicesExtensions;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();



var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddIdentitySetup()
  .AddJwtAuthenticationSetup(builder.Configuration)
  .AddAuthorization()
  .AddCustomPolicies();

  builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(
            telemetryConfiguration: services.GetRequiredService<TelemetryConfiguration>(),
            telemetryConverter: TelemetryConverter.Traces
        );
});
builder.Services
  .AddEndpointsApiExplorer()
  .AddDependencyInjectionSetup(builder.Configuration)
  .AddWriteDbContextSetup(builder.Configuration)
  .AddReadDbContextSetup(builder.Configuration)
  .AddFastEndpointSetup()
  .AddOpenApiSetup();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpointsSetup();
app.MapOpenApi();
app.UseScalarSetup();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.Services.ApplyMigrationsExtension();

// Seed roles after migrations
using (var scope = app.Services.CreateScope())
{
  await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

app.Run();