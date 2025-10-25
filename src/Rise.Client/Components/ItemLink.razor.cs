using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components;

public partial class ItemLink : ComponentBase
{
    [Parameter, EditorRequired] public string Href { get; set; } = null!;
    [Parameter, EditorRequired] public string Icon { get; set; }
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } 
}