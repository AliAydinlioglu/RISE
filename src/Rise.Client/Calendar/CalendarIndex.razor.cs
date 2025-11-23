using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared;
using Rise.Shared.Calendar;

namespace Rise.Client.Calendar;

[HomeBlock(icon:@Icons.Material.Outlined.CalendarMonth,route:"/kalender",label:"Kalender")]
public partial class CalendarIndex
{
    private const string DummyUserId = "1";

    private IEnumerable<CalendarViewItem> _calendarItems = [];
    private bool _isLoading;
    
    protected DateTime _selectedDate;
    protected int _currentView;
    protected List<RenderFragment> _carouselItems = [];
    private bool _showError;

    [Inject] public required ICalendarService CalendarService { get; set; }
    [Inject] public required IDateTimeService DateTimeService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _selectedDate = DateTimeService.Today;

        var result = await CalendarService.GetCalendarAsync(DummyUserId);
        MapResult(result);

        _isLoading = false;
    }

    private void MapResult(Result<CalendarResponse.Get> result)
    {
        if (result.IsSuccess)
        {
            _calendarItems = result.Value.ToCalendarListItems();
            InitializeCarouselItems();
        }
        else
        {
            _showError = true;
        }
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

    private void SetDateRelativeToCurrentDate(int days)
    {
        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
    }

    private IEnumerable<CalendarViewItem> GetItemsForSelectedDate()
    {
        var targetDate = _selectedDate.Date;
        return _calendarItems.Where(i => i.Date.Date == targetDate);
    }

    protected IEnumerable<CalendarViewItem> GetCoursesForSelectedDate()
    {
        return GetItemsForSelectedDate()
            .Where(i => i.Type == CalendarViewItem.CalendarEventType.Course);
    }

    protected IEnumerable<CalendarViewItem> GetAllDeadlines()
    {
        return _calendarItems
            .Where(i => i.Type == CalendarViewItem.CalendarEventType.Deadline)
            .OrderBy(i => i.Date);
    }

    protected void OnViewChanged(int newIndex)
    {
        _currentView = newIndex;
    }

    protected string GetCurrentViewTitle()
    {
        return _currentView switch
        {
            1 => "Lessenrooster",
            2 => "Deadlines",
            _ => "Kalender"
        };
    }
}