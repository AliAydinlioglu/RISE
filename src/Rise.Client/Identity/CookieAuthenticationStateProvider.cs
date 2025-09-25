using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity
{
    /// <summary>
    /// Handles state for cookie-based auth.
    /// </summary>
    /// <remarks>
    /// Create a new instance of the auth provider.
    /// </remarks>
    /// <param name="httpClientFactory">Factory to retrieve auth client.</param>
    public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory): AuthenticationStateProvider, IAccountManager
    {
        /// <summary>
        /// Special auth client.
        /// </summary>
        private readonly HttpClient httpClient = httpClientFactory.CreateClient("SecureApi");
        
        /// <summary>
        /// Authentication state.
        /// </summary>
        private bool authenticated = false;

        /// <summary>
        /// Default principal for anonymous (not authenticated) users.
        /// </summary>
        private readonly ClaimsPrincipal unauthenticated = new(new ClaimsIdentity());

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result serialized to a <see cref="Result"/>.
        /// </returns>
        public async Task<Result> RegisterAsync(string email, string password, string confirmPassword)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/identity/accounts/register", new AccountRequest.Register
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                });
            
                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result!;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not register user.");
                return Result.Error("An unknown error prevented registration from succeeding.");
            }
         }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
        public async Task<Result> LoginAsync(string email, string password)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/identity/accounts/login", new AccountRequest.Login
                {
                    Email = email,
                    Password = password,
                });
            
                var result = await response.Content.ReadFromJsonAsync<Result>();
                if (response.IsSuccessStatusCode)
                {
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }

                return result!;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not login user.");
            }

            return Result.Error("Invalid email and/or password.");
        }

        /// <summary>
        /// Get authentication state.
        /// </summary>
        /// <remarks>
        /// Called by Blazor anytime and authentication-based decision needs to be made, then cached
        /// until the changed state notification is raised.
        /// </remarks>
        /// <returns>The authentication state asynchronous request.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            authenticated = false;
            // default to not authenticated
            var user = unauthenticated;

            try
            {
                var result = await httpClient.GetFromJsonAsync<Result<AccountResponse.Info>>("/api/identity/accounts/info");
                
                if (result!.IsSuccess)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, result.Value.Email),
                        new(ClaimTypes.Email, result.Value.Email)
                    };
                
                    claims.AddRange(
                        result.Value.Claims
                            .Where(c => c.Key is not (ClaimTypes.Name or ClaimTypes.Email or ClaimTypes.Role))
                            .Select(c => new Claim(c.Key, c.Value))
                    );
                
                    claims.AddRange(result.Value.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
                
                    var identity = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                    user = new ClaimsPrincipal(identity);
                    authenticated = true;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode != HttpStatusCode.Unauthorized)
            {
                Log.Error(ex, "Could not GetAuthenticationStateAsync.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not GetAuthenticationStateAsync.");
            }

            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            await httpClient.PostAsync("/api/identity/accounts/logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return authenticated;
        }
    }
}
