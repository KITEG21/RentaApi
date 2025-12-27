using System;

namespace Renta.Domain.Interfaces.Shared;

public interface IQueryRequest
{
    public int? Page { get; set; }
    public int? PerPage { get; set; }
    public string? Query { get; set; }
    public IEnumerable<ISortRequest>? Sorts { get; set; }
    public IEnumerable<IFilterRequest>? Filters { get; set; } 
}