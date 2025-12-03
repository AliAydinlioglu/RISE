using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Client.StudentActivities;

[HomeBlock(icon:@Icons.Material.Outlined.EventNote, label:"Activititeiten", route:"/student-activities")]
public partial class Index
{
    private IEnumerable<StudentActivityDto.Index>? studentActivities;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }
    private int _currentPage = 1;
    private readonly int _pageSize = 8;
    private int _totalCount = 0;
    private int _totalPages => (int)Math.Ceiling((double)_totalCount / _pageSize);
    protected override async Task OnInitializedAsync()
    {
        await LoadStudentActivitiesAsync();
    }

    private async Task LoadStudentActivitiesAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = (_currentPage - 1) * _pageSize,
            Take = _pageSize,
        };

        var result = await StudentActivityService.GetIndexAsync(request, CancellationToken.None);
        studentActivities = result.Value.StudentActivities;
        _totalCount = result.Value.TotalCount;
    }

    private async Task OnPageChangedAsync(int page)
    {
        _currentPage = page;
        await LoadStudentActivitiesAsync();
    }
}