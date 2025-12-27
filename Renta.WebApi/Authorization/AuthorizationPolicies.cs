using Microsoft.AspNetCore.Authorization;

namespace Renta.WebApi.Authorization;

/// <summary>
/// Custom authorization policies for role-based access control
/// </summary>
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string UserOrAdmin = "UserOrAdmin";
    public const string ManagerOnly = "ManagerOnly";

    public static void AddCustomPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AdminOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("role", "Admin");
            })
            .AddPolicy(UserOrAdmin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("role", "User", "Admin");
            })
            .AddPolicy(ManagerOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("role", "Manager");
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