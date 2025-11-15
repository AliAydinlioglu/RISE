using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class RisePagination : ComponentBase
{
    [Parameter] public int PageSize { get; set; } = 8;
    [Parameter] public int TotalPages { get; set; } = 1;
    [Parameter] public int CurrentPage { get; set; } = 1;
    
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }

    private async Task OnPageChanged(int page)
    {
        Log.Information("{0}: {1}", nameof(OnPageChanged), page);
        CurrentPage = page;
        
        await CurrentPageChanged.InvokeAsync(page);
    }
}