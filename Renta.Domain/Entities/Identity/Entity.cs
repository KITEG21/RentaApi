using System;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Identity;

public class Entity : IEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime Created { get; set; }

    public DateTime? LastModified { get; set; }
    public StatusEntityType StatusBaseEntity { get; set; } = StatusEntityType.Active;
}