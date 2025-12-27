using System;
using System.Security.Claims;

namespace Renta.Application.Interfaces;

public interface IActiveUserSession
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserEmail();
    IEnumerable<string> GetUserRoles();
    ClaimsPrincipal? GetCurrentUserPrincipal();
}