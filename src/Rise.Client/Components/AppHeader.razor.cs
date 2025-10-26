using Microsoft.AspNetCore.Components;
using Rise.Client.Shared;

namespace Rise.Client.Components;

public partial class AppHeader : ComponentBase, IDisposable
{
    
    [Parameter, EditorRequired] public RenderFragment IconMenu { get; set; }
    [Parameter, EditorRequired] public RenderFragment ProfileSection { get; set; }
    
    [Inject] private IPageTitleService TitleState { get; set; } = null!;
    
    protected override void OnInitialized()
    {
        TitleState.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        TitleState.OnChange -= StateHasChanged;
    }
    
}