using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant.Components;
[HomeBlock(label:"Resto prijslijst", icon:@Icons.Material.Filled.ReceiptLong, route: "/restaurant/prijslijst")]
public partial class Pricelist : ComponentBase
{
    private RestoOverviewDto _currentResto;
    private bool _visible;
    private IEnumerable<PriceListCategoryDto> _pricelistCategories = [];
    private bool _isError;
    private bool _isLoading;
    [Inject] public required IRestaurantSelectionService RestaurantSelectionService { get; set; }
    [Inject] public required IPriceListService PriceListService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentResto = await RestaurantSelectionService.GetSelectedRestoAsync();
        await LoadPricelistAsync();
    }

    private async Task LoadPricelistAsync()
    {
        try
        {
            _isLoading = true;
            _isError = false;
            var result = await PriceListService.GetForRestoAsync(new MenuRequest.Resto() { Id = _currentResto.Id },
                CancellationToken.None);
            if (result.IsSuccess)
                _pricelistCategories = result.Value.PricelistCategories;
        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _pricelistCategories = null;
        }
        catch (Exception ex)
        {
            _pricelistCategories = null;
            _isError = true;
            Log.Error(ex, "Error loading pricelist for resto {RestoName} ", _currentResto.Name);
        }
        finally
        {
            _isLoading = false;
        }
    }
    private void ToggleRestoSelector()
    {
        _visible = !_visible;
    }

    private async Task ApplyRestoSelection(RestoOverviewDto resto)
    {
        _currentResto = resto;
        await RestaurantSelectionService.SetSelectedRestoAsync(resto);
        _visible = false;
        await LoadPricelistAsync();
    }
}