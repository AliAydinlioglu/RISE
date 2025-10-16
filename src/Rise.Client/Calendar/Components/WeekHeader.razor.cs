using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Calendar.Components;

public partial class WeekHeader
{
    private const int SchooldaysPerWeek = 5;
    
    [Parameter] public DateTime SelectedDate { get; set; }
    [Parameter] public string Title { get; set; } = string.Empty;
    
    [Parameter] public EventCallback<DateTime> SelectedDateChanged { get; set; }
    [Parameter] public EventCallback OnPreviousWeek { get; set; }
    [Parameter] public EventCallback OnNextWeek { get; set; }

    private async Task OnDateSelected(DateTime date)
    {
        SelectedDate = date;
        await SelectedDateChanged.InvokeAsync(date);
    }

    private IEnumerable<DateTime> GetSchooldaysOfCurrentWeek()
    {
        var monday = CalendarHelpers.GetMondayOfWeek(SelectedDate);
        return Enumerable.Range(0, SchooldaysPerWeek)
            .Select(dayOffset => monday.AddDays(dayOffset));
    }

    private int GetWeekNumber()
    {
        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            SelectedDate,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday
        );
    }
}