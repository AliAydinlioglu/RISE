using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using Rise.Client.Identity;
using Rise.Client.Theme;

namespace Rise.Client.Layout;

public partial class MainLayout
{
    [Inject]
    private SignOutSessionStateManager SignOutManager { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private IThemingService ThemingService { get; set; } = default!;
    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }
    private MudTheme _mytheme = new MudTheme();

    private void GoToAccountSettings()
    {
        NavigationManager.NavigateTo("/settings");
    }
    private void GoToAccount()
    {
        NavigationManager.NavigateTo("/account");
    }

    private async Task LogoutAsync()
    {
        await SignOutManager.SetSignOutState();
        NavigationManager.NavigateTo("authentication/logout");
    }

    void GoToLogin() { NavigationManager.NavigateTo("/login"); }

    private string UserInitials { get; set; } = "?";
    private string UserName { get; set; } = "";

    protected override async Task OnParametersSetAsync()
    {
        if (AuthenticationState is not null)
        {
            var authState = await AuthenticationState;
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                UserName = user.Identity.Name
                    ?? user.FindFirst("name")?.Value
                    ?? user.FindFirst("preferred_username")?.Value
                    ?? user.FindFirst("email")?.Value
                    ?? "User";

                UserInitials = GetInitials(UserName);
            }
        }
    }

    protected override void OnInitialized()
    {
        ThemingService.Subscribe += (sender, args) => StateHasChanged();
        _mytheme = ThemingService.Theme;
        ThemingService.Initialize();
    }
    
    private string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "?";

        if (name.Contains("@"))
            name = name.Split('@')[0];

        var parts = name.Split(new[] { ' ', '.', '_' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return "?";

        if (parts.Length == 1)
        {
            return parts[0].Length >= 2
                ? parts[0].Substring(0, 2).ToUpperInvariant()
                : parts[0].ToUpperInvariant();
        }

        return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpperInvariant();
    }
}