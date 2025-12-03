using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;
using Rise.Client.Services;

namespace Rise.Client.StudentActivities;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Activititeiten", route: "/student-activities")]
public partial class Index
{
    private IEnumerable<StudentActivityDto.Index>? _studentActivities;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required IPaginationStateService PaginationStateService { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    
    private const string PageKey = "student-activities-page";
    private int _currentPage = 1;
    private const int PageSize = 8;
    private int _totalCount = 0;
    private int TotalPages => (int)Math.Ceiling((double)_totalCount / PageSize);

    protected override async Task OnInitializedAsync()
    {
        var saved = PaginationStateService.GetPage(PageKey);
        if (saved.HasValue)
        {
            _currentPage = saved.Value;
        }

        await LoadStudentActivitiesAsync();
    }

    private async Task LoadStudentActivitiesAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = (_currentPage - 1) * PageSize,
            Take = PageSize,
        };

        var result = await StudentActivityService.GetIndexAsync(request, CancellationToken.None);
        _studentActivities = result.Value.StudentActivities;
        _totalCount = result.Value.TotalCount;
    }

    private async Task OnPageChangedAsync(int page)
    {
        _currentPage = page;
        PaginationStateService.SetPage(PageKey, page);
        await LoadStudentActivitiesAsync();
        
        await JsRuntime.InvokeVoidAsync("window.scrollTo", 0, 0);
    }

    private void NavigateToDetail(int id)
    {
        PaginationStateService.SetPage(PageKey, _currentPage);
        NavigationManager.NavigateTo($"/student-activities/{id}");
    }
}