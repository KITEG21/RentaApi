using System;

namespace Renta.Domain.Interfaces.Repositories;

public interface ICommitable: IDisposable
{
    bool IsDisposed { get; }
    int Commit();
    Task<int> CommitAsync();
}