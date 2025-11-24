using Rise.Client.Restaurant.Components;

namespace Rise.Client.Restaurant;

public class RestaurantSelectionStateService : IRestaurantSelectionService
{
    public RestoOverviewDTO SelectedResto { get; private set; }
    private bool _initialized;

    public async Task OnInitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;

        // TODO:  load favorite resto from server


        // todo get default resto from server
        SelectedResto = new RestoOverviewDTO()
        {
            RestoId = 1,
            Name = "Schoonmeersen B",
        };
    }

    public async Task<RestoOverviewDTO> GetSelectedRestoAsync()
    {
        await OnInitializeAsync();
        return SelectedResto;
    }

    public async Task SetSelectedRestoAsync(RestoOverviewDTO resto)
    {
        SelectedResto = resto;
    }
}

public interface IRestaurantSelectionService
{
    Task<RestoOverviewDTO> GetSelectedRestoAsync();
    Task SetSelectedRestoAsync(RestoOverviewDTO resto);
}