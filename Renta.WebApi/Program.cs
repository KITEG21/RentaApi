using Microsoft.EntityFrameworkCore;
using Renta.Infrastructure.Persistence.Context;
using Renta.WebApi;
using Renta.WebApi.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
  .AddEndpointsApiExplorer()
  .AddDependencyInjectionSetup(builder.Configuration)
  .AddWriteDbContextSetup(builder.Configuration)
  .AddReadDbContextSetup(builder.Configuration)
  .AddFastEndpointSetup()
  .AddSwaggerSetup()
  .AddDbContextExtension(builder.Configuration);

  builder.Services.AddDbContext<ApplicationWriteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WriteDefaultConnection")));

builder.Services.AddDbContext<ApplicationReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpointsSetup()
  .UseHttpsRedirection();

app.Services.ApplyMigrationsExtension();


app.Run();

