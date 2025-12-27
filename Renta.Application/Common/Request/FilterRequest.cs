using System;
using Renta.Domain.Interfaces.Shared;

namespace Renta.Application.Common.Request;

public class FilterRequest : IFilterRequest
{
    public string Field { get; set; } = string.Empty;
    public string Op { get; set; } = string.Empty;
    public string Value { get; set; } =  string.Empty;
    
    public FilterRequest(string field, string op, string value)
    {
        Field = field;
        Op = op;
        Value = value;
    }
}