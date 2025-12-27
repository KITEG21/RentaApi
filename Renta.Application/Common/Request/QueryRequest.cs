using System;
using Renta.Domain.Interfaces.Shared;
using FastEndpoints;

namespace Renta.Application.Common.Request;

public class QueryRequest: IQueryRequest
{
    public int? Page { get; set; } = 1;
    public int? PerPage { get; set; } = 10;
    public string? Query { get; set; }
    
    [HideFromDocs]
    public IEnumerable<ISortRequest>? Sorts { get; set; } = new List<SortRequest>();
    
    [HideFromDocs]
    public IEnumerable<IFilterRequest>? Filters { get; set; } = new List<FilterRequest>();
}