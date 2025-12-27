using System;
using System.Security.Claims;
using Renta.Application.Extensions;
using Renta.Application.Interfaces;
using Renta.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace Renta.Infrastructure.Concrete;

public class ActiveUserSession : IActiveUserSession
{
    private readonly ClaimsPrincipal? _user;
    private Guid? _currentUserId;
    private string? _currentUserEmail;
    private readonly IUnitOfWork _unitOfWork;

    public ActiveUserSession(IHttpContextAccessor accesor, IUnitOfWork unitOfWork)
    {
        _user = accesor?.HttpContext?.User;
        _unitOfWork = unitOfWork;
    }
    public Guid? GetCurrentUserId() => _currentUserId ??= _user?.GetUserId();
    public string? GetCurrentUserEmail() => _currentUserEmail ??= _user?.GetUserEmail();

    public IEnumerable<string> GetUserRoles() => _user?.ClaimRoles() ?? Enumerable.Empty<string>();


}