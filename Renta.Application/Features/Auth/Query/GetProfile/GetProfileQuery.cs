using FastEndpoints;

namespace Renta.Application.Features.Auth.Query.GetProfile;

public record GetProfileQuery : ICommand<GetProfileResponse>
{
    // This query gets the current user's profile from claims
    // No input parameters needed as it uses the authenticated user context
}
