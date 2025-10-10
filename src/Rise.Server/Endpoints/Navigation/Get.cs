using Rise.Shared.Navigation;
using Rise.Shared.Common;

namespace Rise.Server.Endpoints.Navigation
{
    /// <summary>
    /// Get all Navigation items
    /// See https://fast-endpoints.com/
    /// </summary>
    /// <param name="navigationService"></param>
    public class Get(INavigationService navigationService) : Endpoint<QueryRequest.SkipTake, Result<NavigationResponse.Get>>
    {
        public override void Configure()
        {
            Get("/api/navigation");
            AllowAnonymous();
        }

        public override Task<Result<NavigationResponse.Get>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
        {
            return navigationService.GetAsync(req, ct);
        }
    }
}
