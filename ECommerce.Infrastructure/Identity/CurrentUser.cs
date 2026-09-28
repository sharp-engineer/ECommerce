using System.Security.Claims;
using ECommerce.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Identity;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public Guid? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        User?.FindFirstValue("sub");
            return Guid.TryParse(value, out var userId) ? userId : null;
        }
    }

    public bool IsInRole(string role) => User?.IsInRole(role) == true;
}