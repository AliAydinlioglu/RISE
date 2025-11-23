using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class Pricelist : ComponentBase
{
    
    private RestoOverviewDTO currentResto = new RestoOverviewDTO()
    {
        RestoId = 1,
        Name = "Schoonmeersen B",
    };

    private bool _visible = false;
    
    private void ToggleRestoSelector()
    {
        _visible = !_visible;
    }

    private async Task ApplyRestoSelection(RestoOverviewDTO resto)
    {
        currentResto = resto;
        // TODO: store current selected resto in storage for history
        _visible = false;
        // TODO: load from server
        // await LoadMenuAsync();
    }
}