using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class MenuService(ApplicationDbContext dbContext) : IMenuService
{
    public async Task<Result<MenuResponse.DayMenu>> GetDayMenuAsync(MenuRequest.DayMenu req, CancellationToken ct)
    {
        //TODO: check for default or favorite resto when resto id null

        if (req.RestoId == null)
            return Result.Error("Default or favorite resto is not implemented yet.");

        var dayMenu = await dbContext.Menus
            .Include(m => m.MenuItems
                .Where(mi => !mi.IsDeleted))
                .ThenInclude(mi => mi.MenuCategory)
            .Include(m => m.MenuItems
                .Where(mi => !mi.IsDeleted))
                .ThenInclude(mi => mi.Allergens)
            .Include(m => m.MenuItems
                .Where(mi => !mi.IsDeleted))
            .ThenInclude(mi => mi.DietaryRestrictions)
            .FirstOrDefaultAsync(m => 
                !m.IsDeleted && 
                m.Date == req.Date && 
                m.Resto.Id == req.RestoId, ct);

        if (dayMenu == null)
            return Result.NotFound("DayMenu for this resto and this date was not found.");

        return Result.Success(new MenuResponse.DayMenu
        {
            MenuItemCategories = dayMenu.MenuItems
                .GroupBy(mi => mi.MenuCategory)
                .Select(cat => new MenuItemCategoryDto
                {
                    CategoryName = cat.Key.Name,
                    MenuItems = cat.Select(mi => mi.ToMenuItemDto())
                    .ToArray()
                })
                .ToArray()
        });

    }
}