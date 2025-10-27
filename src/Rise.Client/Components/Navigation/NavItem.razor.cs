using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Navigation;

public partial class NavItem
{
    [Parameter]
    public string Icon { get; set; } = "";
    [Parameter]
    public string Label { get; set; } = "";
    [Parameter]
    public string Link { get; set; } = "#";
    [Parameter]
    public string NavClass { get; set; } = "nav-item"; //nav-card
}

