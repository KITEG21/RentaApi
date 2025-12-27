using System;
using System.Linq.Expressions;

namespace Renta.Domain.Interfaces.Repositories;

public interface IWriteGenericCoreRepository<T> : ICommitable where T : class
{
    void Delete(T entity, bool commit = true);
    void DeleteRange(IEnumerable<T> entities, bool commit = true);
    void Delete(params Expression<Func<T, bool>>[] filters);
    void Save(T entity, bool commit = true);
    void Update(T entity, bool commit = true);
    Task SaveAsync(T entity, bool commit = true);
    Task SaveRangeAsync(IEnumerable<T> entities, bool commit = true);
    Task UpdateAsync(T entity, bool commit = true);
    Task UpdateRangeAsync(IEnumerable<T> entities, bool commit = true);
    Task DeleteAsync(T entity, bool commit = true);
    Task<T?> FirstAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    Task<T?> GetByIdAsync<TKey>(TKey id, bool useInactive = false);
    IQueryable<T> GetAll(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters);
    
}
