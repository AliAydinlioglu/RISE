using Microsoft.AspNetCore.Components;
using Rise.Shared.Courses;

namespace Rise.Client.Courses;

public partial class Detail
{
    private const string DummyUserId = "1";
    [Inject] public required ICourseService CourseService { get; set; }

    [SupplyParameterFromQuery(Name = "datum")] public DateOnly Date { get; set; }

    [Parameter] public required string CourseId { get; set; }
    private CourseDetailViewModels _course = null!;
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var parsedId = int.Parse(CourseId);
        var result = await CourseService.GetCourseDetailAsync(DummyUserId, parsedId, Date);

        _course = result.Value.ToViewModel();
        _isLoading = false;
    }
}
