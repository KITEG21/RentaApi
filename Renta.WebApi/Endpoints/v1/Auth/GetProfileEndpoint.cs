using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Renta.Application.Features.Auth.Query.GetProfile;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Auth;

[Authorize] // This attribute requires a valid JWT token
public class GetProfileEndpoint : CoreEndpointWithoutRequest<GetProfileResponse>
{
    public override void Configure()
    {
        Get("/auth/profile");
        Description(b => b
            .WithTags(RouteGroup.Auth)
            .WithSummary("Get current user profile")
            .WithDescription("Returns the authenticated user's profile information from JWT claims")
        );
        base.Configure();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetProfileQuery();
        var result = await query.ExecuteAsync(ct);
        await Send.OkAsync(result, ct);
    }
}
