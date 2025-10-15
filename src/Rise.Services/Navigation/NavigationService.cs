using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Navigation;

namespace Rise.Services.Navigation;

public class NavigationService(ApplicationDbContext dbContext) : INavigationService
{
    public async Task<Result<NavigationResponse.Get>> GetAsync(NavigationRequest.Get req, Guid roleId, CancellationToken ct)
    {
        if (req == null!)
            return Result.Error("Request is NULL.");
        
        if (string.IsNullOrWhiteSpace(req.ContentLocation))
            return Result.Error("ContentLocation is not given correctly.");
        
        var navigationItems = await dbContext.RoleNavigationItems
            .AsNoTracking()
            .OrderBy(rni => rni.SequenceNr)
            .Where(rni => 
                rni.Id.RoleId == roleId && 
                rni.ContentLocation.Name == req.ContentLocation)
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
}
