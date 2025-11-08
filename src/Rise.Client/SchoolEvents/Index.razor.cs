using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;

namespace Rise.Client.SchoolEvents;

[HomeBlock(icon:Icons.Material.Filled.Event, label:"Evenementen", route:"/school-events")]
public partial class Index 
{
    private IEnumerable<SchoolEventDto.Index>? _schoolEvents;

    private DateTime _selectedDate = DateTime.Now;
    private bool _showError;

    private int _currentPage = 1;
    private const int PageSize = 8;
    private int TotalCount { get; set; }
    private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    [Inject] public required ISchoolEventService SchoolEventService { get; set; }
    
    private async Task LoadSchoolEventsAsync()
    {
        Log.Information("Loading events: date={Date}, skip={Skip}, take={Take}", 
            _selectedDate, (_currentPage - 1) * PageSize, PageSize);
        
        var request = new QueryRequest.SkipTake
        {
            Skip = (_currentPage - 1) * PageSize,
            Take = PageSize,
        };

        var result = await SchoolEventService.GetIndexAsync(request, CancellationToken.None);

        if (result.IsSuccess)
        {
            _schoolEvents = result.Value.SchoolEvents;
            TotalCount = result.Value.TotalCount;
            _showError = false;
        }
        else
        {
            _showError = true;
        }
        
        // re-render
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync() 
        => await LoadSchoolEventsAsync();

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");
        
        _selectedDate = date;
        _currentPage = 1;
        
        await LoadSchoolEventsAsync();
    }
    
    private async Task SetDateRelativeToCurrentDateAsync(int days)
    {
        Log.Information("{0}: {1}", nameof(SetDateRelativeToCurrentDateAsync), days);
        
        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
        _currentPage = 1;

        await LoadSchoolEventsAsync();
    }
    
    private async Task OnPageChangedAsync(int newPage)
    {
        if (_currentPage == newPage) return;
        _currentPage = newPage;
        await LoadSchoolEventsAsync();
    }
}