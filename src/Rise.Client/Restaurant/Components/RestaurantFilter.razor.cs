using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class RestaurantFilter : ComponentBase
{
    [Inject] public required IWeekmenuService WeekmenuService { get; set; }
    [Parameter] public bool IsVisible { get; set; } = false;
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public EventCallback<RestoOverviewDTO> OnSelect { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

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

    private async Task HandleRestaurantSelect(RestoOverviewDTO resto)
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
    
}