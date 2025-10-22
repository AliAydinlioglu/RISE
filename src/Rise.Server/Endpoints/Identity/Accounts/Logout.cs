using Microsoft.AspNetCore.Identity;
using Rise.Persistence.Models.Identity;

namespace Rise.Server.Endpoints.Identity.Accounts;

/// <summary>
/// Logout Endpoint.
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="signInManager"></param>
public class Logout(SignInManager<ApplicationUser> signInManager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/api/identity/accounts/logout");
    }

    public override async Task<Result> HandleAsync(CancellationToken ct)
    {
        await signInManager.SignOutAsync();
        return Result.NoContent();       
    }
}