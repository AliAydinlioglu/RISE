using Microsoft.AspNetCore.Components;

namespace Rise.Client.Courses.Components;

public partial class CourseDetailDeadlines
{
    [Parameter, EditorRequired] public required IList<DeadlineViewModel> Deadlines { get; set; }
}