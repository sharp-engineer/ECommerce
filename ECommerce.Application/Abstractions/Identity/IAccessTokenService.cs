namespace ECommerce.Application.Abstractions.Identity;

public interface IAccessTokenService
{
    AccessTokenResult Generate(AuthenticatedUser user);
}

public sealed record AuthenticatedUser(
    Guid Id,
    string Email,
    IReadOnlyCollection<string> Roles);
    
public sealed record AccessTokenResult(
    string Token,
    DateTime ExpiresAtUtc);