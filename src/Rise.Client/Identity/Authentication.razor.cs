using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Shared.Identity.Accounts;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Rise.Client.Identity;

public partial class Authentication
{
    [Parameter]
    public string? Action { get; set; }

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IAccessTokenProvider TokenProvider { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool showError = false;
    private string errorMessage = string.Empty;

    private async Task OnLoginSucceeded(RemoteAuthenticationState state)
    {
        try
        {
            Log.Information("Login succeeded, calling backend to create/update user");

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var oid = user.FindFirst("oid")?.Value;

            var tokenResult = await TokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                Log.Information("Access token acquired");
                var httpClient = HttpClientFactory.CreateClient("SecureApi");
                var response = await httpClient.PostAsJsonAsync("/api/identity/accounts/login-callback",
                    new AccountRequest.LoginCallback { Oid = oid });

                var result = await response.Content.ReadFromJsonAsync<Result<AccountResponse.LoginCallback>>();
                if (result?.IsSuccess == true)
                {
                    Log.Information("User created/updated successfully: {Email}", result.Value?.Email);

                    var returnUrl = await JSRuntime.InvokeAsync<string?>("localStorage.getItem", "loginReturnUrl");

                    await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "loginReturnUrl");

                    var targetUrl = returnUrl ?? "/kalender";
                    Log.Information("Redirecting to: {TargetUrl}", targetUrl);

                    Navigation.NavigateTo(targetUrl, forceLoad: true);
                    return;
                }
                else
                {
                    var errors = string.Join(", ", result?.Errors ?? Array.Empty<string>());
                    Log.Warning("Failed to create/update user: {Errors}", errors);
                    await HandleLoginError($"Kon gebruiker niet aanmaken of bijwerken: {errors}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error calling login-callback endpoint");
            await HandleLoginError($"Er is een onverwachte fout opgetreden: {ex.Message}");
        }
    }

    private async Task HandleLoginError(string message)
    {
        errorMessage = message;
        showError = true;

        // Force re-render to show error
        StateHasChanged();

        // Wacht 3 seconden zodat gebruiker de error kan zien
        await Task.Delay(3000);

        // Clear alle auth tokens uit localStorage en sessionStorage
        await ClearAuthStorage();

        // Logout bij Entra ID en redirect naar login pagina
        await LogoutFromEntraId();
    }

    private async Task LogoutFromEntraId()
    {
        try
        {
            Log.Information("Logging out from Entra ID...");

            // Redirect naar Entra ID logout endpoint
            var tenantId = "052a5cbb-135d-45c5-b50e-689b56d29142";
            var postLogoutRedirectUri = Uri.EscapeDataString($"{Navigation.BaseUri}login");
            var logoutUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/logout?post_logout_redirect_uri={postLogoutRedirectUri}";

            Navigation.NavigateTo(logoutUrl, forceLoad: true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging out from Entra ID");
            // Fallback: redirect naar login pagina
            Navigation.NavigateTo("/login", forceLoad: true);
        }
    }

    private async Task ClearAuthStorage()
    {
        try
        {
            Log.Information("Clearing auth storage...");

            // Clear localStorage
            await JSRuntime.InvokeVoidAsync("eval", @"
                const keysToRemove = [];
                for (let i = 0; i < localStorage.length; i++) {
                    const key = localStorage.key(i);
                    if (key && (
                        key.includes('msal') || 
                        key.includes('login') || 
                        key.includes('auth') ||
                        key.includes('token') || 
                        key.includes('account') ||
                        key.includes('nonce') ||
                        key.includes('state') ||
                        key.includes('telemetry') ||
                        key.startsWith('msal.') ||
                        key.startsWith('login.') ||
                        key.includes('052a5cbb-135d-45c5-b50e-689b56d29142') ||
                        key.includes('dbcbd040-8ba4-4ed9-9004-fafff85674c7') ||
                        key.includes('0b1720b9-f0e1-43fa-88ea-4b4fecd6352b')
                    )) {
                        keysToRemove.push(key);
                    }
                }
                keysToRemove.forEach(key => localStorage.removeItem(key));
                console.log('Removed', keysToRemove.length, 'localStorage keys');
            ");

            // Clear sessionStorage
            await JSRuntime.InvokeVoidAsync("eval", @"
                const sessionKeysToRemove = [];
                for (let i = 0; i < sessionStorage.length; i++) {
                    const key = sessionStorage.key(i);
                    if (key && (
                        key.includes('msal') || 
                        key.includes('login') || 
                        key.includes('auth') ||
                        key.includes('token') || 
                        key.includes('account')
                    )) {
                        sessionKeysToRemove.push(key);
                    }
                }
                sessionKeysToRemove.forEach(key => sessionStorage.removeItem(key));
                console.log('Removed', sessionKeysToRemove.length, 'sessionStorage keys');
            ");

            Log.Information("Auth storage cleared successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error clearing auth storage");
        }
    }

    private async Task OnLogoutSucceeded(RemoteAuthenticationState state)
    {
        Log.Information("Logout succeeded, redirecting to login");

        // Clear storage ook bij normale logout
        await ClearAuthStorage();

        showError = false;
        errorMessage = string.Empty;
        Navigation.NavigateTo("/login", forceLoad: true);
    }
}