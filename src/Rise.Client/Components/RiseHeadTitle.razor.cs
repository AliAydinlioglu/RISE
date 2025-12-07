using Microsoft.AspNetCore.Components;
using Rise.Client.Shared;
using Rise.Client.Theme;

namespace Rise.Client.Components;

public partial class RiseHeadTitle : ComponentBase
{
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? SubTitle { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public string OverlayColor { get; set; } = "rgba(0,0,0,0.35)";
    
    private string? BackgroundStyle =>
        !ThemingService.ImagesOff && !string.IsNullOrEmpty(ImageUrl)
            ? $"background-image: url('{ImageUrl}'); background-size: cover; background-position: center; color: var(--mud-palette-text-secondary);"
            : "background-color: var(--mud-palette-primary); color: var(--mud-palette-text-secondary);";

    [Inject] private IPageTitleService TitleState { get; set; } = null!;
    
    [Inject] private IThemingService ThemingService { get; set; } = null!;
    
    protected override void OnParametersSet()
    {
        if (ChildContent is not null)
        {
            TitleState.SetTitle(ChildContent);
        }
    }

    protected override void OnInitialized()
    {
        ThemingService.Subscribe += (sender, args) => StateHasChanged();
    }
}