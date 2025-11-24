using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class MenuService(ApplicationDbContext dbContext) : IMenuService
{
    public async Task<Result<MenuResponse.DayMenu>> GetDayMenuAsync(MenuRequest.DayMenu req, CancellationToken ct)
    {
        if (req == null)
            return Result.Error("Request is NULL.");

        //TODO: check for default or favorite resto when resto id null
        var id = req.RestoId ?? 1;

        if (id <= 0)
            return Result.Error("Id has an invalid value.");

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
            .SingleOrDefaultAsync(m => 
                !m.IsDeleted && 
                m.Date == req.Date && 
                m.Resto.Id == id, ct);

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