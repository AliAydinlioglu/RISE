using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;
using Rise.Client.Services;

namespace Rise.Client.SchoolEvents;

[HomeBlock(icon: Icons.Material.Filled.Event, label: "Evenementen", route: "/school-events")]
public partial class Index
{
    private IEnumerable<SchoolEventDto.Index>? _schoolEvents;

    private DateTime _selectedDate = DateTime.Now;

    private int _currentPage = 1;
    private const int PageSize = 8;
    private const string PageKey = "school-events-page";
    private int _totalCount;
    private int TotalPages => (int)Math.Ceiling((double)_totalCount / PageSize);

    [Inject] public required ISchoolEventService SchoolEventService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required IPaginationStateService PaginationStateService { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }


    private async Task LoadSchoolEventsAsync()
    {   
        var request = new QueryRequest.SkipTake
        {
            Skip = (_currentPage - 1) * PageSize,
            Take = PageSize,
            Filters = new Dictionary<string, object?>()
            {
                { "Date", _selectedDate.ToUniversalTime().Date }
            }
        };

        var result = await SchoolEventService.GetIndexAsync(request, CancellationToken.None);

        if (result.IsSuccess)
        {
            _schoolEvents = result.Value.SchoolEvents;
            _totalCount = result.Value.TotalCount;
        }

        Log.Information(
            "Loading events: date={Date}, skip={Skip}, take={Take}, PageSize={TotalCount}, TotalPages={TotalPages}",
            _selectedDate, (_currentPage - 1) * PageSize, PageSize, _totalCount, TotalPages);
    }

    protected override async Task OnInitializedAsync()
    {
        var saved = PaginationStateService.GetPage(PageKey);
        if (saved.HasValue)
        {
            _currentPage =  saved.Value;
        }

        await LoadSchoolEventsAsync();
    }

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");

        _selectedDate = date;
        _currentPage = 1;
        PaginationStateService.SetPage(PageKey, _currentPage);

        await LoadSchoolEventsAsync();
    }

    private async Task SetDateRelativeToCurrentDateAsync(int days)
    {
        Log.Information("{0}: {1}", nameof(SetDateRelativeToCurrentDateAsync), days);

        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
        _currentPage = 1;
        PaginationStateService.SetPage(PageKey, _currentPage);

        await LoadSchoolEventsAsync();
    }

    private async Task OnPageChangedAsync(int newPage)
    {
        if (_currentPage == newPage) return;
        _currentPage = newPage;
        PaginationStateService.SetPage(PageKey, _currentPage);
        await LoadSchoolEventsAsync();
        
        await JsRuntime.InvokeVoidAsync("window.scrollTo", 0, 0);
    }

    private void NavigateToDetail(int id)
    {
        PaginationStateService.SetPage(PageKey, _currentPage);
        NavigationManager.NavigateTo($"/school-events/{id}");
    }
}