using System;

namespace Renta.Domain.Interfaces.Shared;

public interface ISortRequest
{
    public string Field { get; set; }
    public bool IsAsc { get; set; }
}
