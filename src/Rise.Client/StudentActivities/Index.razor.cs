using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;
using Rise.Client.Shared;

namespace Rise.Client.StudentActivities;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Activititeiten", route: "/student-activities")]
public partial class Index : PaginatedComponentBase
{
    private IEnumerable<StudentActivityDto.Index>? _studentActivities;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }
    
    protected override string PageKey => "student-activities-page";
    protected override string RoutePrefix => "student-activities";

    private async Task LoadStudentActivitiesAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = (CurrentPage - 1) * PageSize,
            Take = PageSize,
        };

        var result = await StudentActivityService.GetIndexAsync(request, CancellationToken.None);
        _studentActivities = result.Value.StudentActivities;
        TotalCount = result.Value.TotalCount;
    }
    protected override Task LoadDataAsync() => LoadStudentActivitiesAsync();
    
}