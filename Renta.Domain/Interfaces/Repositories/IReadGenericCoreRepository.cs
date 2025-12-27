using System;
using System.Linq.Expressions;
using Renta.Domain.Interfaces.Shared;

namespace Renta.Domain.Interfaces.Repositories;

public interface IReadGenericCoreRepository<T> where T : class
{
    Task<T?> FirstAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    Task<long> CountAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    T GetById<TKey>(TKey id, bool useInactive = false);
    T First(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    IQueryable<T> GetAll(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    IQueryable<T> GetAllFiltered(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, IQueryRequest? req = null);
    /// <summary>
    /// Gets the elements that matches with the specified ftsQuery using PostgreSQL Full-Text Search  
    /// </summary>
    /// <param name="language"></param>
    /// <param name="useInactive"></param>
    /// <param name="includes"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public IQueryable<T> GetAllFilteredWithFTS(string language, bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, IQueryRequest? req = null);
    long Count(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[] filters);
}
