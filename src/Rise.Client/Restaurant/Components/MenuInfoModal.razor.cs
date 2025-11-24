using Microsoft.AspNetCore.Components;

namespace Rise.Client.Restaurant.Components;

public partial class MenuInfoModal : ComponentBase
{
    [Parameter] public bool Visible {get; set;}
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    
    
    private async Task HandleClose()
    {
        await OnClose.InvokeAsync();
    }
    
    private async Task HandleVisibilityChange(bool isVisible)
    {
       Visible = isVisible;
        await VisibleChanged.InvokeAsync(isVisible);
    }
}