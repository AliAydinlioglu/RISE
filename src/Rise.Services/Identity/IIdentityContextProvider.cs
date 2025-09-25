using System.Security.Claims;

namespace Rise.Services.Identity;

/// <summary>
/// Interace for the session context provider, can be easily mocked in test case scenarios.
/// So if you're testing a Service that uses the session provider, you can mock the provider.
/// For example in <see cref="ProjectService"/>.
/// </summary>
public interface ISessionContextProvider
{
    ClaimsPrincipal? User { get; } 
}