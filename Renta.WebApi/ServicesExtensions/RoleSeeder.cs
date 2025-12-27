using Microsoft.AspNetCore.Identity;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;

namespace Renta.WebApi.ServicesExtensions;

public static class RoleSeeder
{
    public static readonly string[] AvailableRoles = ["Admin", "Dealer", "Client"];

    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        foreach (var roleName in AvailableRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant(),
                    Created = DateTime.UtcNow,
                    StatusBaseEntity = StatusEntityType.Active,
                    IsLock = false
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}
