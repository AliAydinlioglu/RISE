using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Identity;
using Rise.Shared.Navigation;

namespace Rise.Services.Navigation;

public class NavigationService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager) : INavigationService
{
    public async Task<Result<NavigationResponse.Get>> GetAsync(ClaimsPrincipal? principal, QueryRequest.SkipTake req, CancellationToken ct)
    {
        var roleGuid = new Guid(AppRoles.Regular);
        if (principal != null)
        {
            var roleName = principal.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(roleName))
            {
                Log.Error("Role is unknown.");
                return Result.NotFound("Role is unknown.");
            }
        
            var role = await roleManager.FindByNameAsync(roleName!);
            if (role == null)
            {
                Log.Error("Role is not found.");
                return Result.NotFound("Role is not found.");
            }
            
            roleGuid = new Guid(role.Id);
        }
        
        var navigationItems = await dbContext.RoleNavigationItems
            .AsNoTracking()
            .OrderBy(rni => rni.SequenceNr)
            .Where(rni => rni.RoleId == roleGuid)
            .Select(rni => new NavigationDto.Get
            {
                Label = rni.NavigationItem.Label,
                Icon = rni.NavigationItem.Icon,
                Url = rni.NavigationItem.Url,
            })
            .ToListAsync(ct);
            
        return Result.Success(new NavigationResponse.Get
        {
            NavigationItems = navigationItems,
            TotalCount = navigationItems.Count
        });
    }
}
