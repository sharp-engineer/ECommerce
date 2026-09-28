namespace ECommerce.Application.Abstractions.Identity;

public interface IUserRoleService
{
    Task AddToRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);
}