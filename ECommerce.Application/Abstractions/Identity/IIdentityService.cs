namespace ECommerce.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<AuthenticatedUser?> AuthenticateAsync(string email, string password,
        CancellationToken cancellationToken = default);
}