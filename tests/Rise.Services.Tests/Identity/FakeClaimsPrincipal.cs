using System.Security.Claims;

namespace Rise.Services.Tests.Identity;

public static class FakeClaimsPrincipal
{
    public static ClaimsPrincipal Unauthenticated() =>
        new(new ClaimsIdentity());

    public static ClaimsPrincipal WithoutClaims() =>
        new(new ClaimsIdentity([], "SomeAuthType"));

    public static ClaimsPrincipal WithOidOnly(string oid) =>
        new(new ClaimsIdentity([new Claim("oid", oid)], "SomeAuthType"));

    public static ClaimsPrincipal WithClaimsForLoginCallback(string oid, string email, string role) =>
        new(new ClaimsIdentity([
            new Claim("oid", oid),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role)
        ], "SomeAuthType"));
}