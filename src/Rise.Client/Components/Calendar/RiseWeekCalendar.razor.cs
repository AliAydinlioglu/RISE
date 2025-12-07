using Microsoft.AspNetCore.Components;
using Rise.Client.Calendar;

namespace Rise.Client.Components.Calendar;

public partial class RiseWeekCalendar
{
    private const string PrimaryBackgroundColor = "background-color: var(--mud-palette-primary) !important;";
    private const string SecondaryBackgroundColor = "background-color: var(--mud-palette-secondary) !important;";
    private const string PrimaryTextColor = "color: var(--mud-palette-text-secondary) !important;";
    private const string SecondaryTextColor = "color: var(--mud-palette-text-primary) !important;";
    
    [Parameter] public DateTime SelectedDate { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;
    
    [Parameter] public EventCallback<DateTime> SelectedDateChanged { get; set; }
    [Parameter] public EventCallback OnPreviousWeek { get; set; }
    [Parameter] public EventCallback OnNextWeek { get; set; }
    [Parameter] public RiseWeekCalendarType CalendarType { get; set; } = RiseWeekCalendarType.Primary;
    [Parameter] public RiseWeekCalendarWidth CalendarWidth { get; set; } = RiseWeekCalendarWidth.Small;
    [Parameter] public bool ShowOnlySchoolDays { get; set; } = true;
    [Parameter] public bool ShowWeek { get; set; } = true;

    private async Task OnDateSelected(DateTime date)
    {
        SelectedDate = date;
        await SelectedDateChanged.InvokeAsync(date);
    }

    private IEnumerable<DateTime> GetSchooldaysOfCurrentWeek()
    {
        var monday = CalendarHelpers.GetMondayOfWeek(SelectedDate);
        return Enumerable.Range(0, GetDaysPerWeekToShow())
            .Select(dayOffset => monday.AddDays(dayOffset));
    }

    private int GetDaysPerWeekToShow()
    {
        return ShowOnlySchoolDays ? 5 : 7;
    }

    private string GetBackgroundColorClass() => CalendarType switch
    {
        RiseWeekCalendarType.Primary => PrimaryBackgroundColor,
        RiseWeekCalendarType.Secondary => SecondaryBackgroundColor,
        _ => string.Empty
    };
    
    private string GetForegroundColorClass() => CalendarType switch
    {
        RiseWeekCalendarType.Primary => PrimaryTextColor,
        RiseWeekCalendarType.Secondary => SecondaryTextColor,
        _ => string.Empty
    };

    private string GetBackgroundColorClassWhenSelected(bool isSelected)
    {
        return CalendarType switch
        {
            RiseWeekCalendarType.Primary => isSelected 
                ? SecondaryBackgroundColor 
                :  PrimaryBackgroundColor,
            RiseWeekCalendarType.Secondary => isSelected 
                ? PrimaryBackgroundColor 
                :  SecondaryBackgroundColor,
            _ => string.Empty
        };
    } 
    
    private string GetForegroundColorClassWhenSelected(bool isSelected)
    {
        return CalendarType switch
        {
            RiseWeekCalendarType.Primary => isSelected 
                ? SecondaryTextColor 
                : PrimaryTextColor,
            RiseWeekCalendarType.Secondary => isSelected 
                ? PrimaryTextColor 
                : SecondaryTextColor,
            _ => string.Empty
        };
    } 
    
    private string GetWidthClass()
    {
        return CalendarWidth switch
        {
            RiseWeekCalendarWidth.Small => "rise-calendar-width-small",
            RiseWeekCalendarWidth.Medium => "rise-calendar-width-medium",
            RiseWeekCalendarWidth.Large => "rise-calendar-width-large",
            _ => string.Empty
        };
    } 
    
    public enum RiseWeekCalendarType
    {
        Primary,
        Secondary
    }
    
    public enum RiseWeekCalendarWidth
    {
        Small,
        Medium,
        Large
    }
}