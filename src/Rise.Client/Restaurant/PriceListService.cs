using System.Net.Http.Json;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class PriceListService(HttpClient httpClient) : IPriceListService
{
    public async Task<Result<MenuResponse.Pricelist>> GetForRestoAsync(MenuRequest.Resto req, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<MenuResponse.Pricelist>>($"/api/menu/pricelist?Id={req.Id}", ctx);
        return result!;
    }
}