using System.Security.Claims;
using Rise.Services.Identity;

namespace Rise.TestDoubles.Fakers;

public class FakeSessionContextProvider(ClaimsPrincipal? user = null) : ISessionContextProvider
{
    public ClaimsPrincipal? User { get; } = user;
}