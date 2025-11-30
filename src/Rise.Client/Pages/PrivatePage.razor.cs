using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Rise.Client.Pages;

public partial class PrivatePage
{
    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = default!;

    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private List<string> roles = new();
    [CascadingParameter]
    private Task<AuthenticationState>? AuthState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (AuthState == null)
        {
            return;
        }
        var authState = await AuthState;
        claims = authState.User.Claims;
        var httpClient = HttpClientFactory.CreateClient("SecureApi");
        var info = await httpClient.GetFromJsonAsync<Rise.Shared.Identity.Accounts.AccountResponse.Info>("/api/identity/accounts/info");
        roles = info?.Claims.Select(c => c.Value).ToList() ?? [];
        // roles = claims
        //     .Where(c => c.Type == ClaimTypes.Role || c.Type == "roles")
        //     .Select(c => c.Value)
        //     .Distinct()
        //     .ToList();
    }
}
