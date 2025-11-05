using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Shared.Identity.Accounts;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;

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
                Log.Information("Access token acquired: {Token}", token.Value);

                var httpClient = HttpClientFactory.CreateClient("SecureApi");

                var response = await httpClient.PostAsJsonAsync("/api/identity/accounts/login-callback", new AccountRequest.LoginCallback{ Oid = oid });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result<AccountResponse.LoginCallback>>();

                    if (result?.IsSuccess == true)
                    {
                        Log.Information("User created/updated successfully: {Email}", result.Value?.Email);
                    }
                    else
                    {
                        Log.Warning("Failed to create/update user: {Errors}", string.Join(", ", result?.Errors ?? Array.Empty<string>()));
                    }
                }
                else
                {
                    Log.Error("Failed to create/update user: {StatusCode}", response.StatusCode);
                }
            }
            else
            {
                Log.Warning("No access token available — user not authenticated?");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error calling login-callback endpoint");
        }
    }
    private void OnLogoutSucceeded(RemoteAuthenticationState state)
    {
        Log.Information("Logout succeeded, redirecting to calendar");
        Navigation.NavigateTo("/kalender", forceLoad: true);
    }
}