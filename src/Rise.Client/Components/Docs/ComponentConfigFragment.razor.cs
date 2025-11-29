using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Docs;

public partial class ComponentConfigFragment
{
    [Parameter] public string ConfigTitle { get; set; } = "Voorbeeld";
    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; }
}