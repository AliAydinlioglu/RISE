using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class Pricelist : ComponentBase
{

    private RestoOverviewDTO currentResto;
    private bool _visible;
    [Inject] public required IRestaurantSelectionService RestaurantSelectionService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        currentResto = await RestaurantSelectionService.GetSelectedRestoAsync();
    }

    private void ToggleRestoSelector()
    {
        _visible = !_visible;
    }

    private async Task ApplyRestoSelection(RestoOverviewDTO resto)
    {
        currentResto = resto;
        await RestaurantSelectionService.SetSelectedRestoAsync(resto);
        _visible = false;
        // TODO: load pricelist from server
        // await LoadMenuAsync();
    }
}