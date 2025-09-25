using System.Security.Claims;
using Rise.Services.Identity;

namespace Rise.Server.Identity;

/// <summary>
/// Provides the current user from the HttpContext to the session provider.
/// Basically so you can use the session provider in the Service layer.
/// <see cref="ISessionContextProvider"/>
/// </summary>
/// <param name="httpContextAccessor"></param>
public class HttpContextSessionProvider(IHttpContextAccessor httpContextAccessor) : ISessionContextProvider 
{ 
    public ClaimsPrincipal? User => httpContextAccessor!.HttpContext?.User; 
} 
