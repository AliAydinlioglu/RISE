using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Client.StudentActivities;

public partial class Index
{
    private IEnumerable<StudentActivityDto.Index>? studentActivities;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }
   
    protected override async Task OnInitializedAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 12,
        };

        var result = await StudentActivityService.GetIndexAsync(request, CancellationToken.None);
        studentActivities = result.Value.StudentActivities;
    }
}