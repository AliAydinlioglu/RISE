using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Identity;
using Rise.Shared.Navigation;

namespace Rise.Services.Navigation;

public class NavigationService(
    ApplicationDbContext dbContext, 
    ISessionContextProvider sessionContextProvider,
    RoleManager<IdentityRole> roleManager) : INavigationService
{
    public async Task<Result<NavigationResponse.Get>> GetAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        var roleGuid = new Guid(await GetRoleGuidStringAsync(sessionContextProvider.User));
        
        var navigationItems = await dbContext.RoleNavigationItems
            .AsNoTracking()
            .OrderBy(rni => rni.SequenceNr)
            .Where(rni => rni.Id.RoleId == roleGuid)
            .Take(req.Take)
            .Select(rni => new NavigationDto.Get
            {
                Label = rni.NavigationItem.Label,
                Icon = rni.NavigationItem.Icon,
                Url = rni.NavigationItem.Url
            })
            .ToListAsync(ct);
            
        return Result.Success(new NavigationResponse.Get
        {
            NavigationItems = navigationItems
        });
    }
    
    /// <summary>
    /// Decides which role guid is needed based on User
    /// </summary>
    /// <param name="principal">User in context</param>
    /// <returns>Role GUID as a string</returns>
    /// TODO: Move to authenticationService when implemented
    private async Task<string> GetRoleGuidStringAsync(ClaimsPrincipal? principal)
    {
        if (principal == null)
            return AppRoles.Public;
        
        var roleName = principal.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleName))
            return AppRoles.Public;
        
        var role = await roleManager.FindByNameAsync(roleName!);
        return role == null 
            ? AppRoles.Public 
            : role.Id;
    }
}
