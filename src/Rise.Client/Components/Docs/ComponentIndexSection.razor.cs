using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Docs;

public partial class ComponentIndexSection
{
    [Parameter, EditorRequired] public required string ComponentTitle { get; set; }
    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; }
}