using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Client.Shared;

namespace Rise.Client.Faker;

public class FakeAccessTokenProvider : IAccessTokenProvider
{
    public ValueTask<AccessTokenResult> RequestAccessToken()
    {
        return RequestAccessToken(new AccessTokenRequestOptions());
    }

    public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
    {
        var token = new AccessToken();
        // Return a successful result with a fake token
        var result = new AccessTokenResult(AccessTokenResultStatus.Success, token, null);
        return ValueTask.FromResult(result);
    }
}
