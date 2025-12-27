using System;
using Renta.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Renta.Domain.Entities.Identity;

public class User : IdentityUser<Guid>, IEntity
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsLock { get; set; } = false;
    public StatusEntityType StatusBaseEntity { get; set ; }
    public UserType UserType { get; set; }
}