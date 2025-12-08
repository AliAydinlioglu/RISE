using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Identity;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

[Authorize]
public class Roles(IUserService userService) : EndpointWithoutRequest<Result<AccountResponse.Roles>>
{
    public override void Configure()
    {
        Get("/api/identity/accounts/roles");
    }

    public override async Task<Result<AccountResponse.Roles>> ExecuteAsync(CancellationToken ct)
    {
        return await userService.GetUserRolesAsync();
    }
}