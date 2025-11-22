using Microsoft.AspNetCore.Components;

namespace Rise.Client.Courses.Components;

public partial class CourseDetailGeneralInfo
{
    [Parameter, EditorRequired] public required CourseDetailViewModels Course { get; set; }
}