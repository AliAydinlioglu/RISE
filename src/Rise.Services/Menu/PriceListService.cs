using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class PriceListService(ApplicationDbContext dbContext) : IPriceListService
{
    public async Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        if (req == null)
            return Result.Error("Request is NULL.");

        //TODO: check for default or favorite resto when resto id null
        var id = req.Id ?? 1;

        if (id <= 0)
            return Result.Error("Id has an invalid value.");

        var resto = await dbContext.Restos
            .Include(r => r.PriceList)
                .ThenInclude(pl => pl.PriceListItems.Where(pli => !pli.IsDeleted))
                .ThenInclude(pli => pli.PriceListCategory)
            .SingleOrDefaultAsync(r =>
                !r.IsDeleted &&
                r.Id == id, ct);

        if (resto == null)
            return Result.NotFound("Resto was not found.");

        if (resto.PriceList == null)
            return Result.Success(new MenuResponse.Pricelist());

        return Result.Success(new MenuResponse.Pricelist
        {
            PricelistCategories = resto.PriceList
                .PriceListItems
                .GroupBy(pl => pl.PriceListCategory)
                .Select(plc => new PriceListCategoryDto
                {
                    CategoryName = plc.Key.Name,
                    PriceListItems = plc
                        .Select(pli => pli.ToPriceListItemDto())
                        .ToArray()
                })
                .ToArray()
        });
    }
}