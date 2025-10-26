using Microsoft.AspNetCore.Components;

namespace Rise.Client.Shared;

public interface IPageTitleService
{
    void SetTitle(RenderFragment? childContent);
}

public class PageTitleService: IPageTitleService
{
    public RenderFragment? Content { get; private set; }
    public event Action? OnChange;


    public void SetTitle(RenderFragment? childContent)
    {
        Content = childContent;
        OnChange?.Invoke();
    }
}