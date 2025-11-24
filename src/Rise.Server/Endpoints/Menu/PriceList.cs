using Rise.Shared.Menu;

namespace Rise.Server.Endpoints.Menu;

public class PriceList
{
    public class Index(IPriceListService priceListService) : Endpoint<MenuRequest.Resto, Result<MenuResponse.Pricelist>>
    {
        public override void Configure()
        {
            Get("/api/menu/pricelist");
            AllowAnonymous();
        }

        public override Task<Result<MenuResponse.Pricelist>> ExecuteAsync(MenuRequest.Resto req, CancellationToken ct)
        {
            return priceListService.GetForRestoAsync(req, ct);
        }
    }
}