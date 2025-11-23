using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class RestaurantFilter : ComponentBase
{
    [Inject] public required IWeekmenuService WeekmenuService { get; set; }
    [Parameter] public bool IsVisible { get; set; } = false;
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public EventCallback<RestoOverviewDTO> OnSelect { get; set; }
    protected override async Task OnInitializedAsync()
    {
       await getRestaurants();
    }
    
    private ISet<RestoOverviewDTO> Restaurants = new HashSet<RestoOverviewDTO>() ;

    private async Task getRestaurants()
    {
        var result = await WeekmenuService.getRestoOverviews();
        if (result.IsSuccess)
        {
            Restaurants = result.Value;
        }
    }

    private async Task OnRestaurantSelect(RestoOverviewDTO resto)
    {
         await OnSelect.InvokeAsync(resto);
    }
    
    private async Task HandleVisibilityChange(bool isVisible)
    {
        // Update the local state (optional, but good practice)
        
        IsVisible = isVisible;
        
        // CRITICAL: Tell the parent the overlay closed!
        await IsVisibleChanged.InvokeAsync(isVisible);
    }
    
}