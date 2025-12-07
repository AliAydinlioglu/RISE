using Microsoft.AspNetCore.Components;
using Rise.Client.Restaurant.Components;
using Rise.Shared.Common;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class RestaurantSelectionStateService : IRestaurantSelectionService
{
    public RestoOverviewDto? SelectedResto { get; private set; }
    private bool _initialized;
    private readonly IFavouriteRestoService _favouriteRestoService;
    private readonly IRestoService _restoService;
    public RestaurantSelectionStateService(IFavouriteRestoService favouriteRestoService, IRestoService restoService)
    {
        _favouriteRestoService = favouriteRestoService;
        _restoService = restoService;
    }
    public async Task OnInitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;

        // TODO:  load favorite resto from server
        SelectedResto = await _favouriteRestoService.GetFavouriteRestoAsync();
        if (SelectedResto != null)
            return;

        var result = await _restoService.GetOverviewAsync(new QueryRequest.SkipTake(), CancellationToken.None);
        if (result.IsSuccess)
            SelectedResto = result.Value.Restos[0];
    }

    public async Task<RestoOverviewDto> GetSelectedRestoAsync()
    {
        await OnInitializeAsync();
        return SelectedResto;
    }

    public async Task SetSelectedRestoAsync(RestoOverviewDto resto)
    {
        SelectedResto = resto;
    }
}

public interface IRestaurantSelectionService
{
    Task<RestoOverviewDto> GetSelectedRestoAsync();
    Task SetSelectedRestoAsync(RestoOverviewDto resto);
}