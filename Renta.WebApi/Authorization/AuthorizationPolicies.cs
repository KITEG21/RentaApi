using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Renta.WebApi.Authorization;

/// <summary>
/// Custom authorization policies for role-based access control.
/// Available roles: Admin, Dealer, Client
/// </summary>
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string DealerOnly = "DealerOnly";
    public const string ClientOnly = "ClientOnly";
    public const string DealerOrAdmin = "DealerOrAdmin";
    public const string ClientOrAdmin = "ClientOrAdmin";
    public const string AnyRole = "AnyRole";

        public static void AddCustomPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AdminOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin"); // Use ClaimTypes.Role
            })
            .AddPolicy(DealerOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Dealer"); // Use ClaimTypes.Role
            })
            .AddPolicy(ClientOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Client"); // Use ClaimTypes.Role
            })
            .AddPolicy(DealerOrAdmin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Dealer", "Admin"); // Use ClaimTypes.Role
            })
            .AddPolicy(ClientOrAdmin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Client", "Admin"); // Use ClaimTypes.Role
            })
            .AddPolicy(AnyRole, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin", "Dealer", "Client"); // Use ClaimTypes.Role
            });
    }
}

/// <summary>
/// Example of using policy-based authorization in an endpoint
/// </summary>
/// <example>
/// [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
/// public class AdminOnlyEndpoint : Endpoint<Request, Response>
/// {
///     public override void Configure()
///     {
///         Get("/api/v1/admin/dashboard");
///     }
/// }
/// </example>
/// 