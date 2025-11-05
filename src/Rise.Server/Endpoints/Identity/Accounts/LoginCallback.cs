using Rise.Shared.Identity;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

public class GetOrCreate(IUserService userService) : Endpoint<AccountRequest.LoginCallback, Result<AccountResponse.LoginCallback>>
{
    public override void Configure()
    {
        Post("/api/identity/accounts/login-callback");
    }
    
    public override async Task<Result<AccountResponse.LoginCallback>> ExecuteAsync(AccountRequest.LoginCallback request, CancellationToken ct)
    {
        return await userService.GetOrCreateUserAsync(request.Oid);
    }
}