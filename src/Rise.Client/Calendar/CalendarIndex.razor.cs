using Microsoft.AspNetCore.Components;
using Rise.Shared;
using Rise.Shared.Calendar;

namespace Rise.Client.Calendar;

public partial class CalendarIndex
{
    private const string DummyUserId = "1";
    private const int SchooldaysPerWeek = 5;

    private IEnumerable<CalendarViewItem> _calendarItems = [];
    private DateTime _selectedDate;

    [Inject] public required ICalendarService CalendarService { get; set; }
    [Inject] public required IDateTimeService DateTimeService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // TODO: uncomment line below once full integration is done (for now mock data)
        // _selectedDate = DateTimeService.Today;
        _selectedDate = new DateTime(2024, 11, 13);
        
        var result = await CalendarService.GetCalendarAsync(DummyUserId);
        _calendarItems = result.Value.ToCalendarListItems();
    }

    private void PreviousWeek() => _selectedDate = _selectedDate.AddDays(-7);
    private void NextWeek() => _selectedDate = _selectedDate.AddDays(7);

    private IEnumerable<CalendarViewItem> GetItemsForSelectedDate()
    {
        var targetDate = _selectedDate.Date;
        return _calendarItems!.Where(i => i.Date.Date == targetDate);
    }

    private IEnumerable<DateTime> GetSchooldaysOfCurrentWeek()
    {
        var monday = GetMondayOfWeek(_selectedDate);
        return Enumerable.Range(0, SchooldaysPerWeek)
            .Select(dayOffset => monday.AddDays(dayOffset));
    }

    private static DateTime GetMondayOfWeek(DateTime date)
    {
        var daysFromMonday = (date.DayOfWeek - DayOfWeek.Monday + 7) % 7;
        return date.AddDays(-daysFromMonday);
    }
}