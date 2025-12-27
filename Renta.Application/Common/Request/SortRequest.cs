using System;
using Renta.Domain.Interfaces.Shared;

namespace Renta.Application.Common.Request;

public class SortRequest: ISortRequest
{
    public string Field { get; set; } = string.Empty;
    public bool IsAsc { get; set; }
    
    public SortRequest(string field, bool isAsc)
    {
        Field = field;
        IsAsc = isAsc;
    }
}
