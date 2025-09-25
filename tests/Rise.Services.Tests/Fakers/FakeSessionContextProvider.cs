using System.Security.Claims;
using Rise.Services.Identity;

namespace Rise.Services.Tests.Fakers;

public class FakeSessionContextProvider : ISessionContextProvider
{
    public FakeSessionContextProvider(ClaimsPrincipal user) => User = user;
    public ClaimsPrincipal? User { get; }
}