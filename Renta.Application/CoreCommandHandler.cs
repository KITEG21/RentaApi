using System;
using FastEndpoints;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application;

public abstract class CoreCommandHandler<TRequest, TResponse> :
    CoreApplicationService<TRequest, TResponse> where TRequest : class, ICommand<TResponse>
{
    protected CoreCommandHandler(IActiveUserSession activeUserSession, IUnitOfWork unitOfWork) : base(activeUserSession, unitOfWork)
    {
    }

    protected CoreCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected CoreCommandHandler(IActiveUserSession activeUserSession) : base(activeUserSession)
    {
    }

    protected CoreCommandHandler()
    {
    }
    
    public abstract override Task<TResponse> ExecuteAsync(TRequest command, CancellationToken ct=default);
}
