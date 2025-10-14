using System.Security.Claims;
using Rise.Shared.Common;

namespace Rise.Shared.Navigation;

/// <summary>
/// Provides methods for managing navigation-related operations.
/// </summary>
public interface INavigationService
{
    Task<Result<NavigationResponse.Get>> GetAsync(NavigationRequest.Get req, Guid roleId, CancellationToken ct);
}
