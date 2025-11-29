using Ardalis.Result;
using Rise.Shared.Common;
using Rise.Shared.Menu;
using Rise.Shared.Locations;

namespace Rise.Client.Restaurant;

public class FakeRestoService : IRestoService
{
    public Task<Result<MenuResponse.RestoOverview>> GetOverviewAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return Task.FromResult(Result<MenuResponse.RestoOverview>.Success(new MenuResponse.RestoOverview
        {
            Restos = new[]
            {
                new RestoOverviewDto
                {
                    Id = 1,
                    Name = "Test Resto",
                    IsFavorite = false
                },
                new RestoOverviewDto
                {
                    Id = 2,
                    Name = "New Test Resto",
                    IsFavorite = false
                }
            }
        }));
    }

    public Task<Result<MenuResponse.RestoDetail>> GetDetailAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        return Task.FromResult(Result<MenuResponse.RestoDetail>.Success(new MenuResponse.RestoDetail
        {
            Resto = new RestoDetailDto
            {
                Name = "Test Resto",
                OpeningHours = Array.Empty<RestoOpeningHourDto>(),
                Location = new LocationDto.Index
                {
                    Id = 1,
                    Name = "Test Location",
                    Street = "Test Street",
                    HouseNumber = 1,
                    City = "Test City",
                    Postcode = 1000
                }
            }
        }));
    }

    public Task<Result<MenuResponse.RestoOverview>> SetFavoriteResto(MenuRequest.Resto req, CancellationToken ct)
    {
        return Task.FromResult(Result<MenuResponse.RestoOverview>.Success(new MenuResponse.RestoOverview
        {
            Restos = Array.Empty<RestoOverviewDto>()
        }));
    }
}
