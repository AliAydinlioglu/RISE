using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Rise.Client.Services;

namespace Rise.Client.Shared;

public abstract class PaginatedComponentBase : ComponentBase
{
    
    [Inject] public required IPaginationStateService PaginationStateService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 8;
    protected abstract string PageKey { get;  } 
    protected abstract string RoutePrefix { get; }
    protected int TotalCount { get; set; }
    protected int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    protected override async Task OnInitializedAsync()
    {
        var saved = PaginationStateService.GetPage(PageKey);
        if (saved.HasValue) CurrentPage = saved.Value;
        await LoadDataAsync();
    }
    
    
    protected async Task OnPageChangedAsync(int page)
    {
        if (page == CurrentPage) return;
        CurrentPage = page;
        SaveCurrentPage();
        await LoadDataAsync();
        await JsRuntime.InvokeVoidAsync("window.scrollTo", 0, 0);
    }

    protected void SaveCurrentPage() => PaginationStateService.SetPage(PageKey, CurrentPage);
    
    protected void NavigateToDetail(int id)
    {
        SaveCurrentPage();
        var prefix = RoutePrefix.Trim('/');
        string url = string.IsNullOrEmpty(prefix) ? $"/{id}" : $"/{prefix}/{id}";
        NavigationManager.NavigateTo(url);
    }
    protected abstract Task LoadDataAsync();
}