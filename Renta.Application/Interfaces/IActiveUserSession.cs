using System;

namespace Renta.Application.Interfaces;

public interface IActiveUserSession
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserEmail();
    IEnumerable<string> GetUserRoles();

}