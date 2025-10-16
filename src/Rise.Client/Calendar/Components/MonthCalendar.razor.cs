using Microsoft.AspNetCore.Components;

namespace Rise.Client.Calendar.Components;

public partial class MonthCalendar
{
    [Parameter] public DateTime SelectedDate { get; set; }
    [Parameter] public DateTime DateInView { get; set; }
    [Parameter] public EventCallback<DateTime> DateInViewChanged { get; set; }
    [Parameter] public EventCallback OnPreviousMonth { get; set; }
    [Parameter] public EventCallback OnNextMonth { get; set; }

    private async Task OnDateInViewChanged(DateTime date)
    {
        DateInView = date;
        await DateInViewChanged.InvokeAsync(date);
    }

    private IEnumerable<List<DateTime>> GetWeeksInMonth()
    {
        var firstDayOfMonth = new DateTime(DateInView.Year, DateInView.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        
        var startDate = CalendarHelpers.GetMondayOfWeek(firstDayOfMonth);
        var weeks = new List<List<DateTime>>();
        var currentDate = startDate;

        while (currentDate <= lastDayOfMonth || currentDate.Month == firstDayOfMonth.Month)
        {
            var week = new List<DateTime>();
            for (int i = 0; i < 5; i++)
            {
                if (currentDate.Month == DateInView.Month)
                {
                    week.Add(currentDate);
                }
                else
                {
                    week.Add(DateTime.MinValue);
                }
                currentDate = currentDate.AddDays(1);
            }
            
            currentDate = currentDate.AddDays(2);
            
            if (week.Any(d => d != DateTime.MinValue))
            {
                weeks.Add(week);
            }

            if (currentDate.Month != DateInView.Month && weeks.Count > 0)
            {
                break;
            }
        }

        return weeks;
    }
}