using System;
using System.Security.Claims;

namespace Renta.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        List<string>? result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Role);
    }
    
    public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
{
    if (claimsPrincipal == null || claimsPrincipal.Identity?.IsAuthenticated != true)
    {
        Console.WriteLine("User not authenticated or null.");
        return null;
    }

    Console.WriteLine("All claims: " + string.Join(", ", claimsPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}")));
    
    var claim = claimsPrincipal.FindFirst("canonical_user_id")
                ?? claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)
                ?? claimsPrincipal.FindFirst("sub")
                ?? claimsPrincipal.FindFirst("user_id")
                ?? claimsPrincipal.FindFirst("userId");

    Console.WriteLine($"Found claim: {claim?.Type}: {claim?.Value}");
    if (claim == null) return null;
    if (Guid.TryParse(claim.Value, out var userId)) return userId;
    return null;
}
    
    public static string? GetUserEmail(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Email)?.FirstOrDefault();
}