using System.Globalization;
using Microsoft.AspNetCore.Components;
using Rise.Client.Calendar;

namespace Rise.Client.Components.Calendar;

public partial class RiseWeekCalendar
{
    [Parameter] public DateTime SelectedDate { get; set; } 
    [Parameter] public string Title { get; set; } = string.Empty;
    
    [Parameter] public EventCallback<DateTime> SelectedDateChanged { get; set; }
    [Parameter] public EventCallback OnPreviousWeek { get; set; }
    [Parameter] public EventCallback OnNextWeek { get; set; }
    [Parameter] public RiseWeekCalendarType CalendarType { get; set; } = RiseWeekCalendarType.Primary;
    [Parameter] public RiseWeekCalendarWidth CalendarWidth { get; set; } = RiseWeekCalendarWidth.Small;
    [Parameter] public bool ShowOnlySchoolDays { get; set; } = true;
    [Parameter] public bool EnableWeekNav { get; set; } = true;

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

    private int GetWeekNumber()
    {
        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            SelectedDate,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday
        );
    }
    
    private string GetBackgroundColorClass() => CalendarType switch
    {
        RiseWeekCalendarType.Primary => "has-background-black",
        RiseWeekCalendarType.Secondary => "has-background-white",
        _ => string.Empty
    };
    
    private string GetForegroundColorClass() => CalendarType switch
    {
        RiseWeekCalendarType.Primary => "has-text-white",
        RiseWeekCalendarType.Secondary => "has-text-black",
        _ => string.Empty
    };

    private string GetBackgroundColorClassWhenSelected(bool isSelected)
    {
        return CalendarType switch
        {
            RiseWeekCalendarType.Primary => isSelected 
                ? "has-background-white" 
                :  "has-background-black",
            RiseWeekCalendarType.Secondary => isSelected 
                ? "has-background-black" 
                :  "has-background-white",
            _ => string.Empty
        };
    } 
    
    private string GetForegroundColorClassWhenSelected(bool isSelected)
    {
        return CalendarType switch
        {
            RiseWeekCalendarType.Primary => isSelected 
                ? "has-text-black" 
                : "has-text-white",
            RiseWeekCalendarType.Secondary => isSelected 
                ? "has-text-white" 
                : "has-text-black",
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