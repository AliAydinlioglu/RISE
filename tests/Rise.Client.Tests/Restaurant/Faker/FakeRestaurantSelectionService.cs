using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class FakeRestaurantSelectionService : IRestaurantSelectionService
{
    private RestoOverviewDto _selected = new RestoOverviewDto
    {
        Id = 1,
        Name = "Test Resto",
        IsFavorite = false,
    };

    public Task<RestoOverviewDto> GetSelectedRestoAsync()
    {
        return Task.FromResult(_selected);
    }

    public Task SetSelectedRestoAsync(RestoOverviewDto resto)
    {
        _selected = resto;
        return Task.CompletedTask;
    }
}