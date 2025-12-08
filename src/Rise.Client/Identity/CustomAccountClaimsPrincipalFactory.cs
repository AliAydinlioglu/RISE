using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Rise.Client.Identity;

// Inspiration links
// https://github.com/dotnet/aspnetcore/issues/44692
// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-6.0#customize-the-user

public class CustomAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomAccountClaimsPrincipalFactory(
        IAccessTokenProviderAccessor accessor,
        IHttpClientFactory httpClientFactory)
        : base(accessor)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
        RemoteUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        // take all original claims 
        var principal = await base.CreateUserAsync(account, options);

        if (!(principal.Identity?.IsAuthenticated ?? false))
            return principal;

        var claimsIdentity = (ClaimsIdentity)principal.Identity;
        var httpClient = _httpClientFactory.CreateClient("SecureApi");
        var response = await httpClient.GetAsync("/api/identity/accounts/roles");
        var result = await response.Content.ReadFromJsonAsync<Result<Rise.Shared.Identity.Accounts.AccountResponse.Roles>>();

        if (result?.IsSuccess == true)
        {
            var customRoles = result.Value.Values;

            Log.Information("Roles: " + string.Join(", ", customRoles));
            
            foreach (var role in customRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        return principal;
    }
}
