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
    
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var parsedId = int.Parse(CourseId);
        var result = await CourseService.GetCourseDetailAsync(DummyUserId, parsedId, Date);
        
        MapResult(result);
        _isLoading = false;
    }

    private void MapResult(Result<CourseDetailResponse.Get> result)
    {
        if (result.IsSuccess)
            _course = result.Value.ToViewModel();
        else
            MapErrorStatus(result);
    }

    private void MapErrorStatus(Result<CourseDetailResponse.Get> result)
    {
        _errorMessage = result.Status switch
        {
            ResultStatus.Forbidden => "U hebt geen machtigingen om het detail van deze les op te vrage,",
            ResultStatus.NotFound => "Geen lessen gevonden voor cursus op geselecteerde dag.",
            ResultStatus.Invalid => "De opgegeven datum valt buiten het academisch semester waarin deze les valt.",
            _ => "Er trad een onverwachte fout op. Gelieve later opnieuw te proberen"
        };
    }
}
