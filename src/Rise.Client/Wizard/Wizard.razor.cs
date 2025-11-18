using Microsoft.AspNetCore.Components;

namespace Rise.Client.Wizard;

public partial class Wizard : ComponentBase
{
    [Parameter] public bool IsDarkTheme { get; set; }
    [Parameter] public bool ShowImages { get; set; }
    [Parameter] public bool IsOnsideLayout { get; set; }
    [Parameter] public bool IsHighContrast { get; set; }

    [Parameter] public EventCallback<bool> IsDarkThemeChanged { get; set; }
    [Parameter] public EventCallback<bool> ShowImagesChanged { get; set; }
    [Parameter] public EventCallback<bool> IsOnsideLayoutChanged { get; set; }
    [Parameter] public EventCallback<bool> IsHighContrastChanged { get; set; }

    private async Task OnThemeToggle()
    {
        IsDarkTheme = !IsDarkTheme;
        await IsDarkThemeChanged.InvokeAsync(IsDarkTheme);
    }

    private async Task OnImagesToggle()
    {
        ShowImages = !ShowImages;
        await ShowImagesChanged.InvokeAsync(ShowImages);
    }

    private async Task OnLayoutToggle()
    {
        IsOnsideLayout = !IsOnsideLayout;
        await IsOnsideLayoutChanged.InvokeAsync(IsOnsideLayout);
    }

    private async Task OnContrastToggle()
    {
        IsHighContrast = !IsHighContrast;
        await IsHighContrastChanged.InvokeAsync(IsHighContrast);
    }
}