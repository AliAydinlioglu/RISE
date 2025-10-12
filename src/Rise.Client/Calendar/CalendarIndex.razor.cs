using Microsoft.AspNetCore.Components;
using Rise.Shared;
using Rise.Shared.Calendar;

namespace Rise.Client.Calendar;

public partial class CalendarIndex
{
    private const string DummyUserId = "1";

    private IEnumerable<CalendarViewItem> _calendarItems = [];
    private DateTime _selectedDate;
    private int _currentView;
    private List<RenderFragment> _carouselItems = [];

    [Inject] public required ICalendarService CalendarService { get; set; }
    [Inject] public required IDateTimeService DateTimeService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // TODO: uncomment line below once full integration is done (for now mock data)
        // _selectedDate = DateTimeService.Today;
        _selectedDate = new DateTime(2024, 11, 13);

        var result = await CalendarService.GetCalendarAsync(DummyUserId);
        _calendarItems = result.Value.ToCalendarListItems();
        
        InitializeCarouselItems();
    }

    private void InitializeCarouselItems()
    {
        _carouselItems = new List<RenderFragment>
        {
            RenderCalendarView(),
            RenderLessenroosterView(),
            RenderDeadlineView()
        };
    }

    private void PreviousWeek()
    {
        _selectedDate = _selectedDate.AddDays(-7);
        InitializeCarouselItems();
    }

    private void NextWeek()
    {
        _selectedDate = _selectedDate.AddDays(7);
        InitializeCarouselItems();
    }

    private IEnumerable<CalendarViewItem> GetItemsForSelectedDate()
    {
        var targetDate = _selectedDate.Date;
        return _calendarItems.Where(i => i.Date.Date == targetDate);
    }

    private IEnumerable<CalendarViewItem> GetCoursesForSelectedDate()
    {
        return GetItemsForSelectedDate()
            .Where(i => i.Type == CalendarViewItem.CalendarEventType.Course);
    }

    private IEnumerable<CalendarViewItem> GetAllDeadlines()
    {
        return _calendarItems
            .Where(i => i.Type == CalendarViewItem.CalendarEventType.Deadline)
            .OrderBy(i => i.Date);
    }
    
    private void OnViewChanged(int newIndex)
    {
        _currentView = newIndex;
    }

    private string GetCurrentViewTitle()
    {
        return _currentView switch
        {
            1 => "Lessenrooster",
            2 => "Deadlines",
            _ => "Kalender"
        };
    }
}