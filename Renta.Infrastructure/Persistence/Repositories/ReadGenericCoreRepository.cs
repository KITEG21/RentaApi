using System;
using System.Linq.Expressions;
using Renta.Application.Extensions;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Shared;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Renta.Infrastructure.Persistence.Repositories;

public class ReadGenericCoreRepository<T> : IReadGenericCoreRepository<T> where T : class, IEntity
{
    private readonly ApplicationReadDbContext _dbContext;
    public bool IsDisposed { get; private set; }

    public ReadGenericCoreRepository(ApplicationReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public T GetById<TKey>(TKey id, bool useInactive = false) => _dbContext!.Set<T>()!.Find(id)!;
    public T First(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
        => QueryCore(useInactive, includes, filters).FirstOrDefault()!;
    public IQueryable<T> GetAll(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
        => QueryCore(useInactive, includes, filters);
    public long Count(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[] filters)
        => QueryCore(useInactive, includes, filters).Count<T>();

    public async Task<T?> GetByIdAsync<TKey>(TKey id, bool useInactive = false) => await _dbContext.Set<T>().FindAsync(id);
    public async Task<T?> FirstAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
        => await QueryCore(useInactive, includes, filters).FirstOrDefaultAsync();

    public async Task<long> CountAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
         => await QueryCore(useInactive, includes, filters).CountAsync();

    #region IDisposable
    ~ReadGenericCoreRepository()
    {
        Dispose(false);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!this.IsDisposed && disposing && _dbContext != null)
        {
            _dbContext.Dispose();
        }
        this.IsDisposed = true;
    }
    #endregion


    private IQueryable<T> QueryCore(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
    {
        try
        {
            Func<IQueryable<T>, Expression<Func<T, bool>>, IQueryable<T>>? func = ((current, expression) => current.Where(expression));
            DbSet<T> source = _dbContext.Set<T>();

            IQueryable<T> queryable = source.OfType<T>().AsNoTracking();

            if (includes != null)
            {
                Expression<Func<T, object>>[]? array = (includes as Expression<Func<T, object>>[]) ?? includes.ToArray();
                if (!CollectionUtils.IsNullOrEmpty(array))
                {
                    queryable = PerformInclusions(array, queryable);
                }
            }

            if (!CollectionUtils.IsNullOrEmpty(filters))
            {
                queryable = filters!.Aggregate(queryable, func);
            }

            Expression<Func<T, bool>> statusFilter = (T f) =>
                f.StatusBaseEntity == StatusEntityType.Active
                || (useInactive &&
                    (f.StatusBaseEntity == StatusEntityType.Inactive
                    || f.StatusBaseEntity == StatusEntityType.InEdition));


            queryable = queryable.Where(statusFilter);

            return queryable;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    private IQueryable<T> QueryCore(bool useInactive = false, IQueryRequest? req = null, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
    {
        try
        {
            var hasStatusFilter = req?.Filters?.Any(f =>
                f.Field.Equals("StatusBaseEntity", StringComparison.OrdinalIgnoreCase)) ?? false;
            
            Func<IQueryable<T>, Expression<Func<T, bool>>, IQueryable<T>>? func = ((current, expression) => current.Where(expression));
            DbSet<T> source = _dbContext.Set<T>();

            IQueryable<T> queryable = source.OfType<T>().AsNoTracking();

            if (includes != null)
            {
                Expression<Func<T, object>>[]? array = (includes as Expression<Func<T, object>>[]) ?? includes.ToArray();
                if (!CollectionUtils.IsNullOrEmpty(array))
                {
                    queryable = PerformInclusions(array, queryable);
                }
            }

            if (!CollectionUtils.IsNullOrEmpty(filters))
            {
                queryable = filters!.Aggregate(queryable, func);
            }

            if (!hasStatusFilter)
            {
                Expression<Func<T, bool>> statusFilter = (T f) =>
                    f.StatusBaseEntity == StatusEntityType.Active
                    || (useInactive &&
                        (f.StatusBaseEntity == StatusEntityType.Inactive
                         || f.StatusBaseEntity == StatusEntityType.InEdition));
                queryable = queryable.Where(statusFilter);
            }
            
            return queryable;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>>? includes, IQueryable<T> query) =>
        includes!.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

    public IQueryable<T> GetAllFiltered(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, IQueryRequest? req = null)
           => QueryCore(useInactive: useInactive, req: req, includes: includes).ToDynamic(filters: req?.Filters, sorts: req?.Sorts);

    public IQueryable<T> GetAllFilteredWithFTS(string language, bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, IQueryRequest? req = null)
    
    {
        IQueryable<T> baseQuery = QueryCore(useInactive: useInactive, includes: includes);

        string? ftsQuery = req?.Query;
        if (!string.IsNullOrWhiteSpace(ftsQuery) && typeof(T).GetProperty("SearchVector") != null)
        {
            var sanitizedQuery = FullTextSearchExtensions.SanitizeForTsQuery(ftsQuery);
            var formattedFtsQuery = string.Join(" & ",
                sanitizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );
            if (!string.IsNullOrEmpty(formattedFtsQuery))
            {
                baseQuery = baseQuery.Where(e =>
                    EF.Property<NpgsqlTsVector>(e, "SearchVector").Matches(EF.Functions.ToTsQuery(language, formattedFtsQuery))
                );
            }
        }
        return baseQuery.ToDynamic(filters: req?.Filters, sorts: req?.Sorts);
    }
}
