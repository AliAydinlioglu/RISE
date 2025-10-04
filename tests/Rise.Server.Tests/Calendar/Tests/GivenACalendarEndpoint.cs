using System.Security.Claims;
using Ardalis.Result;
using Rise.Server.Endpoints.Calendar;
using Rise.Shared.Calendar;
using Rise.TestDoubles.Fakers;

namespace Rise.Server.Tests.Calendar.Tests;

public class GivenACalendarEndpoint
{
    private readonly ICalendarService _service = new FakeCalendarService();

    [Fact]
    public async Task WhenCallingEndpointAsAuthorizedUser_ThenServiceIsCalledAndReturnsSuccess()
    {
        var endpoint = CreateEndpointWithAuthenticatedUser();

        var result = await endpoint.ExecuteAsync(CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task WhenCallingEndpointAsNonAuthorizedUser_ThenReturnsUnauthorized()
    {
        var endpoint = CreateEndpointWithUnauthenticatedUser();

        var result = await endpoint.ExecuteAsync(CancellationToken.None);

        result.IsUnauthorized().ShouldBeTrue();
        result.Errors.ShouldContain("User not authenticated");
    }

    private GetCalendarEndpoint CreateEndpointWithAuthenticatedUser()
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, "123") };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        var sessionProvider = new FakeSessionContextProvider(user);
        return new GetCalendarEndpoint(_service, sessionProvider);
    }

    private GetCalendarEndpoint CreateEndpointWithUnauthenticatedUser()
    {
        var sessionProvider = new FakeSessionContextProvider(); 
        return new GetCalendarEndpoint(_service, sessionProvider);
    }
}