using System;
using Renta.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Renta.Domain.Entities.Identity;

public class Role : IdentityRole<Guid>, IEntity
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public StatusEntityType StatusBaseEntity { get; set; }
    public bool IsLock { get; set; } = false;

    //TODO check this prop
    StatusEntityType IEntity.StatusBaseEntity { get => StatusBaseEntity; set => StatusBaseEntity = value; }
}