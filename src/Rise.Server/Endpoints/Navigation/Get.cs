using Rise.Services.Identity;
using Rise.Shared.Identity;
using Rise.Shared.Navigation;

namespace Rise.Server.Endpoints.Navigation
{
    /// <summary>
    /// Get all Navigation items
    /// See https://fast-endpoints.com/
    /// </summary>
    /// <param name="navigationService"></param>
    public class Get(
        INavigationService navigationService, 
        IUserService userService,
        ISessionContextProvider sessionContextProvider) : Endpoint<Shared.Navigation.NavigationRequest.Get, Result<NavigationResponse.Get>>
    {
        public override void Configure()
        {
            Get("/api/v1/navigation");
        }

        public override async Task<Result<NavigationResponse.Get>> ExecuteAsync(Shared.Navigation.NavigationRequest.Get req, CancellationToken ct)
        {
            var roleId = await userService.GetRoleIdAsync(sessionContextProvider.User);
            
            return await navigationService.GetAsync(req, roleId, ct);
        }
    }
}
