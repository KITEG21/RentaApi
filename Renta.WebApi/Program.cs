using Microsoft.EntityFrameworkCore;
using Renta.Infrastructure.Persistence.Context;
using Renta.WebApi;
using Renta.WebApi.ServicesExtensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
  .AddEndpointsApiExplorer()
  .AddDependencyInjectionSetup(builder.Configuration)
  .AddWriteDbContextSetup(builder.Configuration)
  .AddReadDbContextSetup(builder.Configuration)
  .AddFastEndpointSetup()
  .AddSwaggerSetup()
  .AddOpenApi();
  // Remove .AddDbContextExtension and the manual AddDbContext calls

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpointsSetup()
  .UseHttpsRedirection();

app.Services.ApplyMigrationsExtension();

app.Run();