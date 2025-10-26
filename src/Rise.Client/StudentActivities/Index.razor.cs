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
    private int currentPage = 1;
    private int pageSize = 8;
    private int totalCount = 0;
    private int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);
    protected override async Task OnInitializedAsync()
    {
        await LoadStudentActivitiesAsync();
    }

    private async Task LoadStudentActivitiesAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = (currentPage - 1) * pageSize,
            Take = pageSize,
        };

        var result = await StudentActivityService.GetIndexAsync(request, CancellationToken.None);
        studentActivities = result.Value.StudentActivities;
        totalCount = result.Value.TotalCount;
    }

    private async Task OnPageChangedAsync(int page)
    {
        currentPage = page;
        await LoadStudentActivitiesAsync();
    }
}