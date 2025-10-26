using Microsoft.AspNetCore.Components;
using Rise.Client.Shared;

namespace Rise.Client.Faker;

public class FakePageTitleService : IPageTitleService
{
    public RenderFragment? Content { get; private set; }
    public event Action? OnChange;
    private List<RenderFragment> SetTitleHistory { get; } = new();

    public void SetTitle(RenderFragment? childContent)
    {
        Content = childContent;
        if (childContent != null) SetTitleHistory.Add(childContent);
        OnChange?.Invoke();
    }
}