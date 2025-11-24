using Rise.Shared.Menu;

namespace Rise.Services.Menu;

public class PriceListService : IPriceListService
{
    public Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}