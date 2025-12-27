using Microsoft.AspNetCore.Identity;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Identity;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Auth.Query.GetProfile;

public class GetProfileQueryHandler : CoreQueryHandler<GetProfileQuery, GetProfileResponse>
{
    private readonly UserManager<User> _userManager;

    public GetProfileQueryHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager) 
        : base(activeUserSession, unitOfWork)
    {
        _userManager = userManager;
    }

    public override async Task<GetProfileResponse> ExecuteAsync(GetProfileQuery query, CancellationToken ct = default)
    {
        var currentUserId = CurrentUserId;
        
        if (currentUserId is null || currentUserId == Guid.Empty)
        {
            ThrowError("User not authenticated", 401);
        }

        var user = await _userManager.FindByIdAsync(currentUserId.ToString()!);
        
        if (user is null)
        {
            ThrowError("User not found", 404);
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new GetProfileResponse
        {
            UserId = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserType = user.UserType.ToString(),
            Roles = roles.ToList()
        };
    }
}