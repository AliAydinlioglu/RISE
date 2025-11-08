using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class RisePagination : ComponentBase
{
    [Parameter] public int PageSize { get; set; } = 8;
    [Parameter] public int TotalPages { get; set; } = 1;

    private int _currentPage = 1;
    
    private void OnPageChanged(int page)
    {
        Log.Information("{0}: {1}", nameof(OnPageChanged), page);
        _currentPage = page;
    }
}