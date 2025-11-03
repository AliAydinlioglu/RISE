using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class RiseHomeBlock : ComponentBase
{
    [Parameter, EditorRequired] public required string Href { get; set; }
    [Parameter, EditorRequired] public required string Icon { get; set; }
    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; } 
}