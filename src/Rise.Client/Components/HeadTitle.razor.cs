using Microsoft.AspNetCore.Components;
using Rise.Client.Shared;

namespace Rise.Client.Components;

public partial class HeadTitle : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? SubTitle { get; set; }
    
    [Inject] private IPageTitleService TitleState { get; set; } = null!;
    
    protected override void OnParametersSet()
    {
        if (ChildContent is not null)
        {
            TitleState.SetTitle(ChildContent);
        }
    }
}