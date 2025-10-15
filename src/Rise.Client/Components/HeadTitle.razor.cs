using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class HeadTitle : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
}