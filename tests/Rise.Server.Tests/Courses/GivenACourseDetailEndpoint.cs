using System.Security.Claims;
using Rise.Server.Endpoints.Courses;
using Rise.Server.Tests.Courses.Tests;
using Rise.Shared.Courses;
using Rise.TestDoubles.Fakers;

namespace Rise.Server.Tests.Courses;

public class GivenACourseDetailEndpoint
{
    private readonly ICourseService _service = new FakeCourseService();

    [Fact]
    public async Task WhenCallingEndpointAsAuthorizedUser_ThenServiceIsCalledAndReturnsSuccess()
    {
        var endpoint = CreateEndpointWithAuthenticatedUser();

        var result = await endpoint.ExecuteAsync(AValidRequest, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    /* TODO: test should be green when auth is implemented
    [Fact]
    public async Task WhenCallingEndpointAsNonAuthorizedUser_ThenReturnsUnauthorized()
    {
        var endpoint = CreateEndpointWithUnauthenticatedUser();

        var result = await endpoint.ExecuteAsync(AValidRequest, CancellationToken.None);

        result.IsUnauthorized().ShouldBeTrue();
        result.Errors.ShouldContain("User not authenticated");
    }
    */

    private static readonly CourseDetailRequest AValidRequest = new()
    { 
        Id = 1, 
        Datum = new DateOnly(2024, 11, 7) 
    };

    private Detail CreateEndpointWithAuthenticatedUser()
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, "123") };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        var sessionProvider = new FakeSessionContextProvider(user);
        return new Detail(_service, sessionProvider);
    }

    private Detail CreateEndpointWithUnauthenticatedUser()
    {
        var sessionProvider = new FakeSessionContextProvider(); 
        return new Detail(_service, sessionProvider);
    }
}