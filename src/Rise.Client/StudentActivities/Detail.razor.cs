using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Client.StudentActivities;

public partial class Detail
{
    private StudentActivityDto.Detail? studentActivity;
    [Inject] public required IStudentActivityService StudentActivityService { get; set; }

    [Parameter] public string? Id { get; set; }

    private string Title { get; set; }
    private string? Description { get; set; } = String.Empty;
    private string Address { get; set; }
    private string LocationName { get; set; }
    private string TimeString { get; set; }
    private string LocalDateString { get; set; }
    private string StudentClubName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return;
        }

        var idValue = int.Parse(Id);
        var result = await StudentActivityService.GetDetailByIdAsync(idValue, CancellationToken.None);
        studentActivity = result.Value.StudentActivity;
        if (studentActivity != null)
        {
            Title = studentActivity.Title;
            Description = studentActivity.Description;
            Address =
                $"{studentActivity.Location.Street} {studentActivity.Location.HouseNumber} {studentActivity.Location.BusNumber}, {studentActivity.Location.Postcode} {studentActivity.Location.City}";
            LocationName = studentActivity.Location?.Name ?? "";
            TimeString = $"{studentActivity.StartTime:HH:mm} - {studentActivity.EndTime:HH:mm}";
            LocalDateString = studentActivity.Date.LocalDateTime.ToString("dd.MM.yyyy");
            StudentClubName = studentActivity.StudentClub.Name;
        }
    }
}