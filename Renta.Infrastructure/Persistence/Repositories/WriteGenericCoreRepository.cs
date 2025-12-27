using System;
using System.Linq.Expressions;
using Renta.Application.Extensions;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Enums;
using Renta.Domain.Interfaces.Repositories;
using Renta.Domain.Interfaces.Shared;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NpgsqlTypes;

namespace Renta.Infrastructure.Persistence.Repositories;

public class WriteGenericCoreRepository<T> : IWriteGenericCoreRepository<T> where T : class, IEntity
{
    private readonly ApplicationWriteDbContext _dbContext;
    public bool IsDisposed { get; private set; }

    public WriteGenericCoreRepository(ApplicationWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected virtual void SaveCore(T entity, bool commit = true)
    {
        DbSet<T> dbSet = _dbContext.Set<T>();
        dbSet.Add(entity);
        if (commit)
        {
            Commit();
        }
    }
    
    protected virtual async Task SaveCoreAsync(T entity, bool commit = true)
    {
        DbSet<T> dbSet = _dbContext.Set<T>();
        await dbSet.AddAsync(entity);
        if (commit)
        {
            await CommitAsync();
        }
    }

    protected virtual void UpdateCore(T entity, bool commit = true)
    {
        _dbContext.Set<T>().Update(entity);
        if (commit)
        {
            Commit();
        }
    }

    protected virtual async Task UpdateCoreAsync(T entity, bool commit = true)
    {
        _dbContext.Set<T>().Update(entity);
        if (commit)
        {
            await CommitAsync();
        }
    }

    protected virtual void DeleteCore(T entity, bool commit = true)
    {
        try
        {
            DbSet<T> dbSet = _dbContext.Set<T>();
            dbSet.Remove(entity);
            if (commit)
            {
                Commit();
            }
        }
        catch (Exception)
        {
            if (_dbContext.ChangeTracker.Entries().Any((EntityEntry q) => q.Entity.Equals(entity) && q.State == EntityState.Deleted))
            {
                _dbContext.Entry<T>(entity).State = EntityState.Unchanged;
            }
            throw;
        }
    }

    protected async virtual Task DeleteCoreAsync(T entity, bool commit = true)
    {
        try
        {
            DbSet<T> dbSet = _dbContext.Set<T>();
            dbSet.Remove(entity);
            if (commit)
            {
                await CommitAsync();
            }
        }
        catch (Exception)
        {
            if (_dbContext.ChangeTracker.Entries().Any((EntityEntry q) => q.Entity.Equals(entity) && q.State == EntityState.Deleted))
            {
                _dbContext.Entry<T>(entity).State = EntityState.Unchanged;
            }
            throw;
        }
    }

    #region ICommitable
    public virtual int Commit() => _dbContext.SaveChanges();
    public virtual async Task<int> CommitAsync() => await _dbContext.SaveChangesAsync();
    #endregion

    public void Delete(params Expression<Func<T, bool>>[] filters)
    {
        DbSet<T> source = _dbContext.Set<T>();
        IQueryable<T> queryable;
        if (CollectionUtils.IsNullOrEmpty(filters))
        {
            queryable = source.AsQueryable<T>();
        }
        else
        {
            queryable = filters.Aggregate(source.OfType<T>(), (IQueryable<T> current, Expression<Func<T, bool>> expression) => current.Where(expression));
        }
        foreach (T current2 in queryable)
        {
            this.DeleteCore(current2);
        }
    }

    public void Delete(T entity, bool commit = true) => DeleteCore(entity, commit);
    public void DeleteRange(IEnumerable<T> entities, bool commit = true)
    {
        foreach (var entity in entities)
        {
            DeleteCore(entity, commit);
        }
    }
    public void Save(T entity, bool commit = true) => SaveCore(entity, commit);
    public void Update(T entity, bool commit = true) => UpdateCore(entity, commit);
    public T GetById<TKey>(TKey id, bool useInactive = false) => _dbContext!.Set<T>()!.Find(id)!;

    public async Task SaveAsync(T entity, bool commit = true) => await SaveCoreAsync(entity, commit);

    public async Task SaveRangeAsync(IEnumerable<T> entities, bool commit = true)
    {
        foreach (var entity in entities)
        {
            await SaveCoreAsync(entity, commit);
        }
    }
    public async Task UpdateAsync(T entity, bool commit = true) => await UpdateCoreAsync(entity, commit);
    public async Task UpdateRangeAsync(IEnumerable<T> entities, bool commit = true)
    {
        foreach (var entity in entities)
        {
            await UpdateCoreAsync(entity, commit);
        }
    }

    public async Task DeleteAsync(T entity, bool commit = true) => await DeleteCoreAsync(entity, commit);
    public async Task<T?> GetByIdAsync<TKey>(TKey id, bool useInactive = false) => await _dbContext.Set<T>().FindAsync(id);
    
    public IQueryable<T> GetAll(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
        => QueryCore(useInactive, includes, filters);
    public async Task<T?> FirstAsync(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, params Expression<Func<T, bool>>[]? filters)
        => await QueryCore(useInactive, includes, filters).FirstOrDefaultAsync();

    #region IDisposable
    ~WriteGenericCoreRepository()
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


            IQueryable<T> queryable = source.OfType<T>();

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

    private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>>? includes, IQueryable<T> query) =>
        includes!.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

    public IQueryable<T> GetAllFiltered(bool useInactive = false, IEnumerable<Expression<Func<T, object>>>? includes = null, IQueryRequest? req = null)
           => QueryCore(useInactive: useInactive, includes: includes).ToDynamic(filters: req?.Filters, sorts: req?.Sorts);

    public IQueryable<T> GetAllFilteredWithFTS(
    string language,
    bool useInactive = false,
    IEnumerable<Expression<Func<T, object>>>? includes = null,
    IQueryRequest? req = null)
    {
        IQueryable<T> baseQuery = QueryCore(useInactive: useInactive, includes: includes);

        string? ftsQuery = req?.Query;
        if (!string.IsNullOrWhiteSpace(ftsQuery) && typeof(T).GetProperty("SearchVector") != null)
        {
            var sanitizedQuery = FullTextSearchExtensions.SanitizeForTsQuery(ftsQuery);
            var formattedFtsQuery = string.Join(" & ", sanitizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList<string>());
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
