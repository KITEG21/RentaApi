namespace Renta.Application.Features.Auth.Query.GetProfile;

public record GetProfileResponse
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public IList<string> Roles { get; init; } = new List<string>();
}