using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Calendar;
using Rise.Shared;
using Rise.Shared.Common;

namespace Rise.Client.Restaurant.Components;

public partial class WeekMenu : ComponentBase
{
    private DateTime _selectedDate;
    private RestoOverviewDTO currentResto = new RestoOverviewDTO()
    {
        RestoId = 1,
        Name = "Schoonmeersen B",
    };
    private bool _visible = false;
    
    private WeekMenuResponse.DayMenu? MenuItems = null;
    
    [Inject] public required IDateTimeService DateTimeService { get; set; }
    [Inject] public required IWeekmenuService WeekmenuService { get; set; }
    [Inject] public required IDialogService? DialogService { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        _selectedDate = DateTimeService.Now;    
        
        if (_selectedDate.DayOfWeek == DayOfWeek.Saturday)
            _selectedDate = _selectedDate.AddDays(2);
        else if (_selectedDate.DayOfWeek == DayOfWeek.Sunday)
            _selectedDate = _selectedDate.AddDays(1);
        
        //TODO: Get favorite resto from user settings else default
        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        var request = new WeekmenuService.WeekMenuRequest.DayMenu
        {
            RestoID = currentResto.RestoId,
            Date = new DateTimeOffset(_selectedDate)
        };

        MenuItems = await WeekmenuService.GetDayMenuAsync(request);
    }

    private void SetDateRelativeToCurrentDate(int days)
    {
        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
    }

    private void OpenRestoSelector()
    {
        _visible = true;
    }
    
    private async Task ApplyRestoSelection(RestoOverviewDTO resto)
    {
        currentResto = resto;
        // TODO: store current selected resto in storage for history
        _visible = false;
        await LoadMenuAsync();
    }

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");
        
        _selectedDate = date;
            
        await LoadMenuAsync();
    }

}