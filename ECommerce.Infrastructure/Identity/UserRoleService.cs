using ECommerce.Application.Abstractions.Identity;
using ECommerce.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

internal sealed class UserRoleService(UserManager<ApplicationUser> userManager) : IUserRoleService
{
    public async Task AddToRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            throw new NotFoundException($"User with id '{userId}' was not found.");

        if (await userManager.IsInRoleAsync(user, role))
            return;

        var result = await userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Failed to assign role '{role}' to user '{userId}': {errors}");
        }
    }
}