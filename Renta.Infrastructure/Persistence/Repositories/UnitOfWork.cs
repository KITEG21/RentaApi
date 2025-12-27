using System;
using System.Data;
using System.Data.Common;
using Renta.Domain.Interfaces.Repositories;
using Renta.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Renta.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationWriteDbContext _writeContext;
    private readonly ApplicationReadDbContext _readContext;
    private readonly Dictionary<Type, object> _readRepositories;
    private readonly Dictionary<Type, object> _writeRepositories;

    private IDbContextTransaction? _transaction;
    protected bool IsDisposed { get; private set; }

    public UnitOfWork(ApplicationWriteDbContext writeContext, ApplicationReadDbContext readContext)
    {
        _writeContext = writeContext;
        _readContext = readContext;
        _readRepositories = new Dictionary<Type, object>();
        _writeRepositories = new Dictionary<Type, object>();
        _transaction = null;
    }
    public IReadGenericCoreRepository<TEntity> ReadDbRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (!_readRepositories.ContainsKey(type))
        {
            var implementationType = typeof(ReadGenericCoreRepository<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(implementationType, _readContext);

            if (instance != null)
            {
                _readRepositories[type] = instance;
            }
        }

        return (IReadGenericCoreRepository<TEntity>)_readRepositories[type];
    }
    public IWriteGenericCoreRepository<TEntity> WriteDbRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (!_writeRepositories.ContainsKey(type))
        {
            var implementationType = typeof(WriteGenericCoreRepository<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(implementationType, _writeContext);

            if (instance != null)
            {
                _writeRepositories[type] = instance;
            }
        }

        return (IWriteGenericCoreRepository<TEntity>)_writeRepositories[type];
    }



    public void BeginTransaction()
    {
        DbConnection connection = _writeContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        _transaction = _writeContext.Database.BeginTransaction(IsolationLevel.Unspecified);
    }

    public void RollBack() => _transaction?.Rollback();

    public bool CommitTransaction()
    {
        _transaction?.Commit();
        return true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                //_logsDbContext.Dispose();
                _writeContext.Dispose();
                _readContext.Dispose();
                _transaction?.Dispose();
            }
            IsDisposed = true;
        }
    }

    public void Save()
    {
        _writeContext.SaveChanges();
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _writeContext.SaveChangesAsync(ct);
    }

    public virtual int CommitChanges(bool autoRollbackOnError = true)
    {
        int result = 0;

        try
        {
            result = _writeRepositories.Values
                .OfType<ICommitable>()
                .Sum(repo => repo.Commit());
        }
        catch (Exception)
        {
            if (autoRollbackOnError)
            {
                RollBack();
            }
            throw;
        }

        return result;
    }

    public virtual async Task<int> CommitChangesAsync(bool autoRollbackOnError = true)
    {
        int result = 0;

        try
        {
            foreach (var repo in _writeRepositories.Values.OfType<ICommitable>())
            {
                result += await repo.CommitAsync();
            }
        }
        catch (Exception)
        {
            if (autoRollbackOnError)
            {
                RollBack();
            }
            throw;
        }

        return result;
    }

    public async Task<bool> CanConnectAsync(CancellationToken ct = default)
    {
        var canWrite = await _writeContext.Database.CanConnectAsync(ct);
        var canRead = await _readContext.Database.CanConnectAsync(ct);
        return canWrite && canRead;
    }

    public void ClearChangeTracker()
    {
        if (_writeContext is ApplicationWriteDbContext writeContext)
        {
            writeContext.ClearChangeTracker();
        }
    }
}