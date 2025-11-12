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
    public async Task<Result<AccountResponse.LoginCallback>> GetOrCreateUserAsync(string oid)
    {
        var claimsPrincipal = sessionProvider.User;

        if (claimsPrincipal is not { Identity.IsAuthenticated: true })
            return Result.Unauthorized("User is not authenticated");

        var email = claimsPrincipal.GetEmail();
        
        if(string.IsNullOrWhiteSpace(email))
            return Result.Error("Email is required");

        if (string.IsNullOrWhiteSpace(oid))
            return Result.Error("Oid is required");
        
        if(!Guid.TryParse(oid, out var oidGuid))
            return Result.Error("Oid is in a wrong format");
        
        var user = await GetApplicationUserAsync(oidGuid, SsoProviders.MicrosoftEntra, email);
        
        // user found => update
        if (user != null)
        {
            // update + return success
            return await TryUpdateUserWithResultAsync(user);
        }
            
        // create new user if not found yet
        user = new ApplicationUser(
            email, 
            claimsPrincipal.GetFirstName(),
            claimsPrincipal.GetLastName(),
            null,
            DateTimeOffset.UtcNow, 
            oidGuid,
            SsoProviders.MicrosoftEntra);

        return await TryCreateUserWithResultAsync(user, oidGuid, SsoProviders.MicrosoftEntra, email);
    }

    /// <summary>
    /// Tries updating an existing user
    /// </summary>
    /// <param name="user">existing user</param>
    /// <returns>Result with DTO or error message</returns>
    private async Task<Result<AccountResponse.LoginCallback>> TryUpdateUserWithResultAsync(ApplicationUser user)
    {
        try
        {
            user.LastLogin = DateTimeOffset.UtcNow;
            await userManager.UpdateAsync(user);
            return Result.Success(await MapToLoginCallbackModelAsync(user));
        }
        catch (Exception e)
        {
            const string publicErrorMessage = "Something went wrong when updating user";
            Log.Error("{PublicErrorMessage}: {Message}", publicErrorMessage, e.GetBaseException().Message);
            return Result.Error(publicErrorMessage);
        }
    }
    
    /// <summary>
    /// Tries creating a new user
    /// </summary>
    /// <param name="user">new user</param>
    /// <param name="oid">unique identifier from SSO</param>
    /// <param name="ssoProvider">Provider of SSO</param>
    /// <param name="email">email of user</param>
    /// <returns>Result with DTO or error message</returns>
    private async Task<Result<AccountResponse.LoginCallback>> TryCreateUserWithResultAsync(ApplicationUser user, Guid oid, string ssoProvider, string email)
    {
        try
        {
            await userManager.CreateAsync(user);
            return Result.Created(await MapToLoginCallbackModelAsync(user));
        }
        catch (DbUpdateException ex) 
            when (ex.GetBaseException().Message.Contains("unique", StringComparison.OrdinalIgnoreCase) || 
                  ex.GetBaseException().Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
        {
            // this catch is triggered by unique key violation. Possible reason: request was sent twice
            var existingUser = await GetApplicationUserAsync(oid, ssoProvider, email);

            if (existingUser != null)
            {
                // user was found now => update + return success
                return await TryUpdateUserWithResultAsync(existingUser);
            }

            // safety 
            const string publicErrorMessage = "User with this info is already present, but for some reason not found.";
            Log.Error("{PublicErrorMessage}: {Message}", publicErrorMessage, ex.GetBaseException().Message);
            return Result.Error(publicErrorMessage);
        }
        catch (Exception e)
        {
            const string publicErrorMessage = "Something went wrong when creating user";
            Log.Error("{PublicErrorMessage}: {Message}", publicErrorMessage, e.GetBaseException().Message);
            return Result.Error(publicErrorMessage);
        }
    }
    
    /// <summary>
    /// Gets user from usermanager
    /// </summary>
    /// <param name="oid">unique identifier from SSO</param>
    /// <param name="ssoProvider">Provider of SSO</param>
    /// <param name="email">email of user</param>
    /// <returns>ApplicationUser or NULL</returns>
    private async Task<ApplicationUser?> GetApplicationUserAsync(Guid oid, string ssoProvider, string email)
    {
        return await userManager.Users
            .SingleOrDefaultAsync(u => 
                (u.SsoId.Equals(oid) && u.SsoProvider == ssoProvider) || 
                u.Email!.Equals(email));
    }
    
    /// <summary>
    /// Maps an user object to DTO object
    /// </summary>
    /// <param name="user">existing user</param>
    /// <returns>DTO object</returns>
    private async Task<AccountResponse.LoginCallback> MapToLoginCallbackModelAsync(ApplicationUser user)
    {
        return new AccountResponse.LoginCallback
        {
            Email = user.Email!,
            Roles = (await userManager.GetRolesAsync(user)).ToArray()
        };
    }
}