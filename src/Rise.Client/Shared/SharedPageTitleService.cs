using Microsoft.AspNetCore.Components;

namespace Rise.Client.Shared;

public class SharedPageTitleService
{
    public RenderFragment? Content { get; private set; }
    public event Action? OnChange;


    public void SetTitle(RenderFragment childContent)
    {
        Content = childContent;
        OnChange?.Invoke();
    }
}