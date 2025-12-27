using System;
using Renta.Domain.Enums;

namespace Renta.Domain.Entities.Identity;

public interface IEntity
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public StatusEntityType StatusBaseEntity { get; set; }
}
