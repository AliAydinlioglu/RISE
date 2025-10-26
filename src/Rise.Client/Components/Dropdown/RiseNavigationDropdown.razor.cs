using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Dropdown;

public partial class RiseNavigationDropdown<TItem> : RiseDropdownBase<TItem>
{
    [Parameter, EditorRequired] public Func<TItem, string> NavigationUrlFunc { get; set; } = null!;
    [Parameter] public string Label { get; set; } = "Selecteer optie";
    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    protected override Task OnItemClick(TItem item)
    {
        var url = NavigationUrlFunc(item);
        if (!string.IsNullOrEmpty(url))
        {
            NavigationManager.NavigateTo(url);
        }
        return Task.CompletedTask;
    }
}