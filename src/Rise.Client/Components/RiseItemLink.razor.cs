using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace Rise.Client.Components;

public partial class RiseItemLink : ComponentBase
{
    [Parameter, EditorRequired] public string Href { get; set; } = null!;
    [Parameter, EditorRequired] public string Icon { get; set; }
    [Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } 
    [Parameter] public NavLinkMatch IsActiveLink { get; set; } 
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public Size Size { get; set; } = Size.Medium;
    
    private string _sizeclass => Size switch
    {
        Size.Small => "itemlink-small",
        Size.Medium => "itemlink-medium",
        Size.Large => "itemlink-large",
        _ => "itemlink-medium"
    };
}