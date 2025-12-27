using System;
using FastEndpoints;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application;

public abstract class CoreQueryHandler<TRequest, TResponse> :
    CoreApplicationService<TRequest, TResponse> where TRequest : class, ICommand<TResponse>
{
    protected CoreQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected CoreQueryHandler()
    {
    }

    protected CoreQueryHandler(IActiveUserSession activeUserSession, IUnitOfWork unitOfWork) : base(activeUserSession,
        unitOfWork)
    {
    }

    public abstract override Task<TResponse> ExecuteAsync(TRequest command, CancellationToken ct = default);
}