using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Dropdown;

public partial class RiseSelectionDropdown<TItem> : RiseDropdownBase<TItem>
{
    [Parameter, EditorRequired] public EventCallback<TItem> SelectedItemChanged { get; set; }
    [Parameter] public TItem? SelectedItem { get; set; }

    protected override async Task OnItemClick(TItem item)
    {
        SelectedItem = item;
        await SelectedItemChanged.InvokeAsync(item);
    }
}