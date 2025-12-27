using System;
using Renta.Domain.Entities.Identity;

namespace Renta.Domain.Entities.Multimedia;

public class Video : Entity
{

    public Guid EntityId { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public int Duration { get; set; } // Duration in seconds

}
