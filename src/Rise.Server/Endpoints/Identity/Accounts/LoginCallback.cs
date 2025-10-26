using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Identity;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

[Authorize]
public class GetOrCreate(IUserService userService) : EndpointWithoutRequest<Result<AccountResponse.LoginCallback>>
{
    public override void Configure()
    {
        Post("/api/identity/accounts/login-callback");
    }
    
    public override async Task<Result<AccountResponse.LoginCallback>> ExecuteAsync(CancellationToken ct)
    {
        return await userService.GetOrCreateUserAsync();
    }
}