using System.Security.Claims;
using Rise.Shared.Identity.Accounts;

namespace Rise.Shared.Identity;

public interface IUserService
{
    Task<Guid> GetRoleIdAsync(ClaimsPrincipal? principal);
    Task<Result<AccountResponse.LoginCallback>> GetOrCreateUserAsync(string oid);
}