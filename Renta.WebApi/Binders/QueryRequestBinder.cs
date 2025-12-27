using FastEndpoints;
using Renta.Application.Common.Request;

namespace Renta.WebApi.Binders;

public class QueryRequestBinder : IRequestBinder<QueryRequest>
{
    private readonly string Page = "page";
    private readonly string PerPage = "perPage";
    private readonly string Query = "query";
    private readonly string Sort = "sorts";
    private readonly char Desc = '-';
    private readonly string Filter = "filters";
    private readonly string DefaultOp = "like";
    public ValueTask<QueryRequest> BindAsync(BinderContext ctx, CancellationToken ct)
    {
        var url = ctx.HttpContext.Request.Query;
        var queryRequest = new QueryRequest();

        if (url.TryGetValue(Page, out var pageStr))
        {
            if (int.TryParse(pageStr, out var page))
            {
                queryRequest.Page = page;
            }
            else
            {
                throw new ArgumentException("Argument page is required to be an integer");
            }
        }

        if (url.TryGetValue(PerPage, out var perPageStr))
            queryRequest.PerPage = int.TryParse(perPageStr, out var perPage) ? perPage : queryRequest.PerPage;

        if (url.TryGetValue(Query, out var qStr))
        {
            var q = qStr.ToString();

            queryRequest.Query = q;
        }

        if (url.TryGetValue(Sort, out var fieldsToSort))
        {
            var sorts = fieldsToSort.Select(x =>
            {
                var sortDir = x[0];
                var field = x.Substring(1);

                return new SortRequest(field, sortDir != Desc);
            });

            queryRequest.Sorts = sorts;
        }

        if (url.TryGetValue(Filter, out var fieldsToFilter))
        {
            var filters = fieldsToFilter.Select(x =>
            {
                var filterArgs = x.Split(',');

                if (filterArgs.Length != 2 &&
                    filterArgs.Length != 3)
                {
                    throw new ArgumentException($"Argument filter {x} has a bad format");
                }

                if (filterArgs.Length == 2)
                {
                    return new FilterRequest(filterArgs[0], DefaultOp, filterArgs[1]);
                }

                return new FilterRequest(filterArgs[0], filterArgs[1], filterArgs[2]);
            });

            queryRequest.Filters = filters;
        }

        return ValueTask.FromResult(queryRequest);
    }
}