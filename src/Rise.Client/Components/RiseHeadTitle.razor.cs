using Microsoft.AspNetCore.Components;
using Rise.Client.Shared;

namespace Rise.Client.Components;

public partial class RiseHeadTitle : ComponentBase
{
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? SubTitle { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public string OverlayColor { get; set; } = "rgba(0,0,0,0.35)";
    
    private string? BackgroundStyle =>
        !string.IsNullOrEmpty(ImageUrl)
            ? $"background-image: url('{ImageUrl}'); background-size: cover; background-position: center; color: var(--mud-palette-text-secondary);"
            : "background-color: var(--mud-palette-primary); color: var(--mud-palette-text-secondary);";

    [Inject] private IPageTitleService TitleState { get; set; } = null!;
    
    protected override void OnParametersSet()
    {
        if (ChildContent is not null)
        {
            TitleState.SetTitle(ChildContent);
        }
    }
}