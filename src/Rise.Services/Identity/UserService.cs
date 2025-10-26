using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence.Models.Identity;
using Rise.Shared.Identity;
using Rise.Shared.Identity.Accounts;

namespace Rise.Services.Identity;

public class UserService(
    RoleManager<ApplicationRole> roleManager, 
    UserManager<ApplicationUser> userManager,
    ISessionContextProvider sessionProvider) : IUserService
{
    /// <summary>
    /// Decides which role guid is needed based on User
    /// </summary>
    /// <param name="principal">User in context</param>
    /// <returns>Role GUID as a string</returns>
    public async Task<Guid> GetRoleIdAsync(ClaimsPrincipal? principal)
    {
        if (principal == null)
            return new Guid(AppRoles.Public);
        
        var roleName = principal.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleName))
            return new Guid(AppRoles.Public);
        
        var role = await roleManager.FindByNameAsync(roleName!);
        return role?.Id ?? new Guid(AppRoles.Public);
    }

    /// <summary>
    /// Gets a user by oid. If not found, a new user is created and known info is returned.
    /// </summary>
    /// <returns>Info of authenticated user</returns>
    public async Task<Result<AccountResponse.LoginCallback>> GetOrCreateUserAsync()
    {
        var claimsPrincipal = sessionProvider.User;

        if (claimsPrincipal is not { Identity.IsAuthenticated: true })
            return Result.Unauthorized("User is not authenticated");

        var oidString = claimsPrincipal.GetOid();

        if (string.IsNullOrWhiteSpace(oidString))
            return Result.Unauthorized("Oid of user is not known and therefore not authenticated");
        
        if(!Guid.TryParse(oidString, out var oid))
            return Result.Error("Oid is in a wrong format");
        
        if (await userManager.Users
            .SingleOrDefaultAsync(u => 
                u.SsoId.Equals(oid) && 
                u.SsoProvider == SsoProviders.MicrosoftEntra) is not { } user)
        {
            // create new user if not present yet
            user = new ApplicationUser(
                claimsPrincipal.GetEmail()!, 
                claimsPrincipal.GetFirstName(),
                claimsPrincipal.GetLastName(),
                null,
                DateTimeOffset.UtcNow, 
                oid,
                SsoProviders.MicrosoftEntra);
            
            await userManager.CreateAsync(user);
            
            return Result.Created(await MapToLoginCallbackModelAsync(user));
        }
        
        user.LastLogin = DateTimeOffset.UtcNow;
        await userManager.UpdateAsync(user);
        
        return Result.Success(await MapToLoginCallbackModelAsync(user));
    }

    private async Task<AccountResponse.LoginCallback> MapToLoginCallbackModelAsync(ApplicationUser user)
    {
        return new AccountResponse.LoginCallback
        {
            Email = user.Email!,
            Roles = (await userManager.GetRolesAsync(user)).ToArray()
        };
    }
}