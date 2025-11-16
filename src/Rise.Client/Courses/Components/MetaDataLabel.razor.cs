using Microsoft.AspNetCore.Components;

namespace Rise.Client.Courses.Components;

public partial class MetaDataLabel
{
    [Parameter, EditorRequired] public required string LabelText { get; set; }
    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; }
}