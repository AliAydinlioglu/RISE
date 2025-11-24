using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class RestoService(ApplicationDbContext dbContext) : IRestoService
{
    public async Task<Result<MenuResponse.RestoOverview>> GetOverviewAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        var restoQuery = dbContext.Restos
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Skip(req.Skip)
            .Take(req.Take)
            .AsQueryable();

        return Result.Success(new MenuResponse.RestoOverview
        {
            Restos = await restoQuery
                .Select(r => new RestoOverviewDto
                {
                    Name = r.Name,
                    IsFavorite = false //TODO
                })
                .ToArrayAsync(ct)
        });
    }

    public async Task<Result<MenuResponse.RestoDetail>> GetDetailAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        if (req.Id <= 0)
            return Result.Error("Id has an invalid value.");
        
        var resto = await dbContext.Restos
            .AsNoTracking()
            .Include(r => r.Location)
            .FirstOrDefaultAsync(r => r.Id == req.Id, ct);
        
        if (resto == null)
            return Result.NotFound("Resto is not found.");
        
        return Result.Success(new MenuResponse.RestoDetail
        {
            Resto = new RestoDetailDto
            {
                Name = resto.Name,
                Location = new LocationDto.Index
                {
                    Id = resto.Location.Id,
                    Name = resto.Location.Name,
                    BusNumber = resto.Location.BusNumber,
                    City = resto.Location.City,
                    HouseNumber = resto.Location.HouseNumber,
                    Street = resto.Location.Street,
                    Postcode = resto.Location.Postcode
                },
                OpeningHours = resto.OpeningHours
                    .Select(oh => new RestoOpeningHourDto
                    {
                        Date = oh.ContactDate,
                        Hours = oh.ContactHours
                            .Select(ch => (ch.StartTime,ch.EndTime))
                            .ToArray()
                    })
                    .ToArray(),
            }
        });
    }

    public Task<Result<MenuResponse.RestoOverview>> SetFavoriteResto(MenuRequest.Resto req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}