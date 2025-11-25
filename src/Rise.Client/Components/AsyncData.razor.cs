using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class AsyncData : ComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public bool IsError { get; set; }
    
}