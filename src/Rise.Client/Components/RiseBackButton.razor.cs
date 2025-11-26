using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class RiseBackButton
{
    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; }
    [Parameter, EditorRequired] public required string NavigateTo { get; set; }
}