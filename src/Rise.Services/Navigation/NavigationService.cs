using Rise.Shared.Common;
using Rise.Shared.Navigation;

namespace Rise.Services.Navigation;

public class NavigationService : INavigationService
{
    public Task<Result<NavigationResponse.Get>> GetAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
