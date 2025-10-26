using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Dropdown;

public abstract class RiseDropdownBase<TItem> : ComponentBase
{
    [Parameter, EditorRequired] public IEnumerable<TItem> Items { get; set; } = null!;
    [Parameter] public Func<TItem, string>? ItemTextFunc { get; set; }
    [Parameter] public string Placeholder { get; set; } = "Selecteer...";
    [Parameter] public RenderFragment? ActivatorContent { get; set; }
    
    protected abstract Task OnItemClick(TItem item);

    protected string GetItemText(TItem? item)
    {
        if (item == null)
            return Placeholder;

        return ItemTextFunc?.Invoke(item) ?? item.ToString()!;
    }
}