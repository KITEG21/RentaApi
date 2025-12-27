using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Renta.Infrastructure.Persistence.Context;
using Renta.WebApi;
using Renta.WebApi.ServicesExtensions;
using Scalar.AspNetCore;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();


var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddIdentitySetup()
  .AddJwtAuthenticationSetup(builder.Configuration)
  .AddAuthorization();

builder.Services
  .AddEndpointsApiExplorer()
  .AddDependencyInjectionSetup(builder.Configuration)
  .AddWriteDbContextSetup(builder.Configuration)
  .AddReadDbContextSetup(builder.Configuration)
  .AddFastEndpointSetup()
  .AddOpenApiSetup();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpointsSetup();

app.Services.ApplyMigrationsExtension();

// Seed roles after migrations
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

app.Run();