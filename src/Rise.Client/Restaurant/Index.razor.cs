using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Client.Restaurant.Components;
using Rise.Client.UserPreferences.Services;
using Rise.Shared;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

[HomeBlock(icon: @Icons.Material.Filled.RestaurantMenu, label: "Weekmenu", route: "/restaurant/weekmenu")]
public partial class Index : ComponentBase
{
    private DateTime _selectedDate;
    private RestoOverviewDto _currentResto = null!;
    private bool _visibleRestaurantSelector;
    private bool _visibleInfoModal;
    private IEnumerable<MenuItemCategoryDto> _menuItems = Enumerable.Empty<MenuItemCategoryDto>();
    private bool _isLoading;
    private bool _isError;

    [Inject] public required IDateTimeService DateTimeService { get; set; }
    [Inject] public required IUserPreferenceStateService PreferenceState { get; set; }
    [Inject] public required IRestoService RestoService { get; set; }
    [Inject] public required IDialogService? DialogService { get; set; }
    [Inject] public required IMenuService MenuService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _selectedDate = DateTimeService.Now;

        if (_selectedDate.DayOfWeek == DayOfWeek.Saturday)
            _selectedDate = _selectedDate.AddDays(2);
        else if (_selectedDate.DayOfWeek == DayOfWeek.Sunday)
            _selectedDate = _selectedDate.AddDays(1);

        await LoadCurrentResto();
        await LoadMenuAsync();
        _isLoading = false;
    }

    private async Task LoadCurrentResto()
    {
        try
        {
            var favoriteRestoId = PreferenceState.FavoriteResto;

            if (favoriteRestoId > 0)
            {
                var result = await RestoService.GetOverviewAsync(
                    new Rise.Shared.Common.QueryRequest.SkipTake(),
                    CancellationToken.None);

                if (result.IsSuccess && result.Value?.Restos != null)
                {
                    _currentResto = result.Value.Restos.FirstOrDefault(r => r.Id == favoriteRestoId);

                    if (_currentResto == null && result.Value.Restos.Any())
                    {
                        _currentResto = result.Value.Restos.First();
                    }
                }
            }
            else
            {
                var result = await RestoService.GetOverviewAsync(
                    new Rise.Shared.Common.QueryRequest.SkipTake(),
                    CancellationToken.None);

                if (result.IsSuccess && result.Value?.Restos != null && result.Value.Restos.Any())
                {
                    _currentResto = result.Value.Restos.First();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading current resto from preferences");
        }
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            _isLoading = true;
            _isError = false;
            var request = new MenuRequest.DayMenu
            {
                RestoId = _currentResto.Id,
                Date = _selectedDate
            };

            var response = await MenuService.GetDayMenuAsync(request, CancellationToken.None);
            if (response.IsSuccess)
            {
                _menuItems = response.Value.MenuItemCategories;
            }
        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _menuItems = null;
        }
        catch (Exception ex)
        {
            _menuItems = null;
            _isError = true;
            Log.Error(ex, "Error loading menu for resto {RestoId} on date {Date}", _currentResto.Id, _selectedDate);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void SetDateRelativeToCurrentDate(int days)
    {
        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
    }

    private void ToggleRestoSelector()
    {
        _visibleRestaurantSelector = !_visibleRestaurantSelector;
    }

    private async Task ApplyRestoSelection(RestoOverviewDto resto)
    {
        _currentResto = resto;
        PreferenceState.FavoriteResto = resto.Id!.Value;
        await PreferenceState.SavePreferencesAsync();
        _visibleRestaurantSelector = false;
        await LoadMenuAsync();
    }

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");

        _selectedDate = date;

        await LoadMenuAsync();
    }

    private void ToggleInfoModal()
    {
        _visibleInfoModal = !_visibleInfoModal;
    }
}