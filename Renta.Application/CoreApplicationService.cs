using System;
using System.Transactions;
using FastEndpoints;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application;

public abstract class CoreApplicationService<TRequest, TResponse> :
    CommandHandler<TRequest, TResponse> where TRequest : class, ICommand<TResponse>
{
    private readonly IActiveUserSession? _activeUserSession;
    protected IUnitOfWork? UnitOfWork { get; set; }

    protected CoreApplicationService(IActiveUserSession activeUserSession, IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        _activeUserSession = activeUserSession;
    }

    protected CoreApplicationService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    protected CoreApplicationService(IActiveUserSession activeUserSession)
    {
        _activeUserSession = activeUserSession;
    }

    protected CoreApplicationService()
    {
    }

    protected Guid? CurrentUserId => _activeUserSession?.GetCurrentUserId() ?? Guid.Empty;
    protected string? CurrentUserEmail => _activeUserSession?.GetCurrentUserEmail() ?? string.Empty;
    protected IEnumerable<string> UserRoles => _activeUserSession?.GetUserRoles() ?? Enumerable.Empty<string>();

    protected static TransactionScope ScopeBeginTransactionAsync()
    {
        return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }

    protected TransactionScope ScopeBeginTransaction(TransactionScopeOption option, IsolationLevel level)
    {
        return new TransactionScope(option, new TransactionOptions
        {
            IsolationLevel = level
        });
    }

    protected static void CommitTransaction(TransactionScope scopeInCurse)
    {
        scopeInCurse.Complete();
    }

    protected async Task<int> CommitAsync()
    {
        return await UnitOfWork!.SaveChangesAsync();
    }
}