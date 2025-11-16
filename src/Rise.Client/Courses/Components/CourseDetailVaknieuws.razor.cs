using Microsoft.AspNetCore.Components;

namespace Rise.Client.Courses.Components;

public partial class CourseDetailVaknieuws
{
    [Parameter, EditorRequired] public required IList<AnnouncementViewModel> Announcements { get; set; }
}