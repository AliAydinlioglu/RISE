using System.Security.Claims;

namespace Rise.Shared.Identity;

public interface IUserService
{
    Task<Guid> GetRoleIdAsync(ClaimsPrincipal? principal);
}