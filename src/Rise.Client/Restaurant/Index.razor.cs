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

[HomeBlock(icon: @Icons.Material.Filled.RestaurantMenu, label: "Weekmenu", route: "/restaurant/")]
public partial class Index : ComponentBase
{
    private DateTime _selectedDate;

    private RestoOverviewDTO currentResto;

    private bool _visibleRestaurantSelector;
    private bool _visibleInfoModal;

    private WeekMenuResponse.DayMenu? MenuItems;
    private bool _isLoading;

    [Inject] public required IDateTimeService DateTimeService { get; set; }
    [Inject] public required IWeekmenuService WeekmenuService { get; set; }
    [Inject] public required IRestaurantSelectionService RestaurantSelectionService { get; set; }
    [Inject] public required IDialogService? DialogService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _selectedDate = DateTimeService.Now;

        if (_selectedDate.DayOfWeek == DayOfWeek.Saturday)
            _selectedDate = _selectedDate.AddDays(2);
        else if (_selectedDate.DayOfWeek == DayOfWeek.Sunday)
            _selectedDate = _selectedDate.AddDays(1);

        currentResto = await RestaurantSelectionService.GetSelectedRestoAsync();
        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        _isLoading = true;
        var request = new WeekmenuService.WeekMenuRequest.DayMenu
        {
            RestoID = currentResto.RestoId,
            Date = new DateTimeOffset(_selectedDate)
        };

        MenuItems = await WeekmenuService.GetDayMenuAsync(request);
        _isLoading = false;
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

    private async Task ApplyRestoSelection(RestoOverviewDTO resto)
    {
        currentResto = resto;
        await RestaurantSelectionService.SetSelectedRestoAsync(resto);
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