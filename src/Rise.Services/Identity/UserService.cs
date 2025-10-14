using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Rise.Domain.Identity;
using Rise.Shared.Identity;

namespace Rise.Services.Identity;

public class UserService(RoleManager<Role> roleManager) : IUserService
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
        return role == null 
            ? new Guid(AppRoles.Public) 
            : role.Id;
    }
}