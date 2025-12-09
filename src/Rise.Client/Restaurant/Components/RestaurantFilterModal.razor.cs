using Microsoft.AspNetCore.Components;
using Rise.Client.UserPreferences.Services;
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
    [Inject] public required IUserPreferenceStateService PreferenceState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetRestaurants();
    }

    private async Task GetRestaurants()
    {
        var result = await RestoService.GetOverviewAsync(new QueryRequest.SkipTake(), CancellationToken.None);
        if (result.IsSuccess)
        {
            _restaurants = result.Value.Restos;

            var favoriteRestoId = PreferenceState.FavoriteResto;
            if (favoriteRestoId > 0)
            {
                var favorite = _restaurants.FirstOrDefault(resto => resto.Id == favoriteRestoId);
                if (favorite != null)
                {
                    favorite.IsFavorite = true;
                }
            }
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

        PreferenceState.FavoriteResto = resto.Id!.Value;

        var saveResult = await PreferenceState.SavePreferencesAsync();

        if (saveResult.IsSuccess)
        {
            await GetRestaurants();
            await OnSelect.InvokeAsync(resto);
        }
    }
}