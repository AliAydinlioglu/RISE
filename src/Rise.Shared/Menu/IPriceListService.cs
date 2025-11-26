namespace Rise.Shared.Menu;

public interface IPriceListService
{
    Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ct);
}