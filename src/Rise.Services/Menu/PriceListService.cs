using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class PriceListService(ApplicationDbContext dbContext) : IPriceListService
{
    public async Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        Log.Information($"{nameof(PriceListService)} - {nameof(GetForRestoAsync)} was called)");
        
        //TODO: check for default or favorite resto when resto id null
        var id = req.Id ?? 1;

        if (id <= 0)
        {
            Log.Error("Id has an invalid value.");
            return Result.Error("Id has an invalid value.");
        }

        var resto = await dbContext.Restos
            .Include(r => r.PriceList)
                .ThenInclude(pl => pl.PriceListItems.Where(pli => !pli.IsDeleted))
                .ThenInclude(pli => pli.PriceListCategory)
            .SingleOrDefaultAsync(r =>
                !r.IsDeleted &&
                r.Id == id, ct);

        if (resto == null)
        {
            Log.Error("Resto was not found in db.");
            return Result.NotFound("Resto was not found.");
        }

        if (resto.PriceList == null)
        {
            Log.Information("Resto doesn't have a pricelist.");
            return Result.Success(new MenuResponse.Pricelist());
        }

        Log.Information("Resto was found and has a pricelist.");
        
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
                        .ToArray(),
                    Remark = plc.Key.Remark
                })
                .ToArray()
        });
    }
}