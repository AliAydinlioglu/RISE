using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant.Components;

public partial class RestaurantFilterModal : ComponentBase
{
    [Parameter] public bool IsVisible { get; set; } = false;
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public EventCallback<RestoOverviewDto> OnSelect { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private IEnumerable<RestoOverviewDto> _restaurants = [];
    [Inject] public required IRestoService RestoService { get; set; }
    [Inject] public required IFavouriteRestoService FavouriteRestoService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await getRestaurants();
    }

    private async Task getRestaurants()
    {
        var result = await RestoService.GetOverviewAsync(new QueryRequest.SkipTake(), CancellationToken.None);
        if (result.IsSuccess) _restaurants = result.Value.Restos;

        //TODO: voorlopig tot userpreferences klaar zijn
        var favResto = await FavouriteRestoService.GetFavouriteRestoAsync();
        if (favResto != null)
        {
            var favorite = _restaurants.FirstOrDefault(resto => resto.Id == favResto.Id);
            if (favorite != null)
                favorite.IsFavorite = true;
        }
    }

    private async Task HandleRestaurantSelect(RestoOverviewDto resto)
    {
        await OnSelect.InvokeAsync(resto);
    }

    private async Task HandleClose()
    {
        await OnClose.InvokeAsync();
    }

    private async Task HandleVisibilityChange(bool isVisible)
    {
        IsVisible = isVisible;
        await IsVisibleChanged.InvokeAsync(isVisible);
    }

    private async Task SelectFavoriteResto(RestoOverviewDto resto)
    {
        if (resto.IsFavorite)
            return;

        var request = new MenuRequest.Resto { Id = resto.Id };
        //TODO: sync with userpreferences
        //var result = await RestoService.SetFavoriteResto(request, CancellationToken.None);
        //if (result.IsSuccess) _restaurants = result.Value.Restos;
        await FavouriteRestoService.SetFavouriteRestoAsync(resto);
        await getRestaurants();
        await OnSelect.InvokeAsync(resto);
    }
}