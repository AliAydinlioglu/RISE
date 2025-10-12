using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Client.StudentActivities;

public partial class Detail 
{
    private StudentActivityDto.Detail? studentActivity;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }
   
    [Parameter]
    public string? Id { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return;
        }
        
        int IdValue = int.Parse(Id);
        var result = await StudentActivityService.GetDetailByIdAsync(IdValue, CancellationToken.None);
        studentActivity = result.Value.StudentActivity;
    }
}