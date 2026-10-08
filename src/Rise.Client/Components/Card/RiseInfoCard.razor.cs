using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Card;

public partial class RiseInfoCard
{
    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Random randomIndex = new Random();
}
