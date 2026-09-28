using ECommerce.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

internal sealed class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<AuthenticatedUser?> AuthenticateAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;

        var passwordValid = await userManager.CheckPasswordAsync(user, password);
        if (!passwordValid) return null;

        if (string.IsNullOrWhiteSpace(user.Email)) return null;

        var roles = await userManager.GetRolesAsync(user);

        return new AuthenticatedUser(
            user.Id,
            user.Email,
            roles.ToArray());
    }
}