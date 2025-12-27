using System;

namespace Renta.Domain.Interfaces.Shared;

public interface IFilterRequest
{
    public string Field { get; set; }
    public string Op { get; set; }
    public string Value { get; set; }
}