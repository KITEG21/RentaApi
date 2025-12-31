using System.IdentityModel.Tokens.Jwt;
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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(
        builder.Configuration["ApplicationInsights:ConnectionString"],
        TelemetryConverter.Traces)
    .CreateLogger();

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


app.Services.ApplyMigrationsExtension();

// Seed roles after migrations
using (var scope = app.Services.CreateScope())
{
  await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

app.Run();