using System;

namespace Renta.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IReadGenericCoreRepository<TEntity> ReadDbRepository<TEntity>() where TEntity : class;
    IWriteGenericCoreRepository<TEntity> WriteDbRepository<TEntity>() where TEntity : class;
    int CommitChanges(bool autoRollbackOnError = true);
    Task<int> CommitChangesAsync(bool autoRollbackOnError = true);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    void RollBack();
    void BeginTransaction();
    bool CommitTransaction();
    Task<bool> CanConnectAsync(CancellationToken ct = default);
    void ClearChangeTracker();
}