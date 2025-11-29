using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class FakeFavouriteRestoService : IFavouriteRestoService
{
    private RestoOverviewDto? _favouriteResto;

    public Task SetFavouriteRestoAsync(RestoOverviewDto resto)
    {
        _favouriteResto = resto;
        return Task.CompletedTask;
    }

    public Task<RestoOverviewDto?> GetFavouriteRestoAsync()
    {
        return Task.FromResult(_favouriteResto);
    }
}

