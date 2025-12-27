using System;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Renta.Application.Wrappers;

public class AsyncQueryableWrapper<T> : IQueryable<T>, IAsyncEnumerable<T>
{
    private readonly IQueryable<T> _innerQueryable;

    public AsyncQueryableWrapper(IQueryable<T> innerQueryable)
    {
        _innerQueryable = innerQueryable;
    }

    public Type ElementType => _innerQueryable.ElementType;

    public Expression Expression => _innerQueryable.Expression;

    public IQueryProvider Provider => new AsyncQueryProviderWrapper<T>(_innerQueryable.Provider);

    public IEnumerator<T> GetEnumerator() => _innerQueryable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IAsyncEnumerator<T> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default)
    {
        var asyncEnumerable = _innerQueryable.AsAsyncEnumerable();
        return asyncEnumerable.GetAsyncEnumerator(cancellationToken);
    }
}

public class AsyncQueryProviderWrapper<T> : IAsyncQueryProvider
{
    private readonly IQueryProvider _innerQueryProvider;

    public AsyncQueryProviderWrapper(IQueryProvider innerQueryProvider)
    {
        _innerQueryProvider = innerQueryProvider;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new AsyncQueryableWrapper<T>(_innerQueryProvider.CreateQuery<T>(expression));
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new AsyncQueryableWrapper<TElement>(_innerQueryProvider.CreateQuery<TElement>(expression));
    }

    public object Execute(Expression expression)
    {
        return _innerQueryProvider.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _innerQueryProvider.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, System.Threading.CancellationToken ct = default)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var executeAsyncMethod = typeof(AsyncQueryProviderWrapper<T>)
            .GetMethod("ExecuteInternalAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .MakeGenericMethod(resultType);
        var result = executeAsyncMethod.Invoke(this, new object[] { expression, ct });
        return (TResult)result;
    }

    private async Task<TResult> ExecuteInternalAsync<TResult>(Expression expression, System.Threading.CancellationToken ct = default)
    {
        var result = await Task.Run(() => _innerQueryProvider.Execute<TResult>(expression), ct).ConfigureAwait(false);
        return result;
    }
}
