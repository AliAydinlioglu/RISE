using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Client.Calendar;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;
using Rise.Client.Shared;

namespace Rise.Client.SchoolEvents;

[HomeBlock(icon: Icons.Material.Filled.Event, label: "Evenementen", route: "/school-events")]
public partial class Index : PaginatedComponentBase
{
    private IEnumerable<SchoolEventDto.Index>? _schoolEvents;

    private DateTime _selectedDate = DateTime.Now;
    protected override string PageKey => "school-events-page";
    protected override string RoutePrefix => "school-events";

    [Inject] public required ISchoolEventService SchoolEventService { get; set; }

    private async Task LoadSchoolEventsAsync()
    {   
        var request = new QueryRequest.SkipTake
        {
            Skip = (CurrentPage - 1) * PageSize,
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
            TotalCount = result.Value.TotalCount;
        }

        Log.Information(
            "Loading events: date={Date}, skip={Skip}, take={Take}, PageSize={TotalCount}, TotalPages={TotalPages}",
            _selectedDate, (CurrentPage - 1) * PageSize, PageSize, TotalCount, TotalPages);
    }

    protected override Task LoadDataAsync() => LoadSchoolEventsAsync();

    private async Task OnDateChangedAsync(DateTime date)
    {
        Log.Information("{0}: {1}", nameof(OnDateChangedAsync), $"{date:dd/MM/yyyy}");

        _selectedDate = date;
        CurrentPage = 1;
        SaveCurrentPage();
        await LoadSchoolEventsAsync();
    }

    private async Task SetDateRelativeToCurrentDateAsync(int days)
    {
        Log.Information("{0}: {1}", nameof(SetDateRelativeToCurrentDateAsync), days);

        _selectedDate = _selectedDate.AddDays(days);
        _selectedDate = CalendarHelpers.GetMondayOfWeek(_selectedDate);
        CurrentPage = 1;
        SaveCurrentPage();
        await LoadSchoolEventsAsync();
    }
}