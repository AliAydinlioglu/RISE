using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Client.Calendar.Components;
using Rise.Client.Components.Calendar;
using Rise.Client.Restaurant.Components;
using Rise.Shared;
using Rise.Shared.Calendar;

namespace Rise.Client.Restaurant;

[HomeBlock(icon:@Icons.Material.Filled.RestaurantMenu, label:"Weekmenu", route:"/restaurant/")]
public partial class Index : ComponentBase
{

    private bool _isLoading;
    
    protected int _currentView;
    protected List<RenderFragment> _carouselItems = [];
    
    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        BuildCarouselItems();
        _isLoading = false;
    }

    private void OnViewChanged(int newIndex)
    {
        _currentView = newIndex;
    }

    private string GetCurrentViewTitle()
    {
        return _currentView switch
        {
            1 => "Prijslijst",
            _ => "Weekmenu"
        };
    }

    private void BuildCarouselItems()
    {
        _carouselItems.Add(builder =>
            {
                builder.OpenComponent(0, typeof(WeekMenu));
                builder.CloseComponent();
            }
        );
        
        _carouselItems.Add(builder =>
            {
                builder.OpenComponent(0, typeof(Pricelist));
                builder.CloseComponent();
            }
        );
    }
}