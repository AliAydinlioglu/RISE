using System.Net.Http.Json;
using Rise.Shared.Common;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class RestoService(HttpClient httpClient) : IRestoService
{
    public async Task<Result<MenuResponse.RestoOverview>> GetOverviewAsync(QueryRequest.SkipTake req, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<MenuResponse.RestoOverview>>($"/api/menu/restos", ctx);
        return result!;
    }

    public async Task<Result<MenuResponse.RestoDetail>> GetDetailAsync(MenuRequest.Resto req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result<MenuResponse.RestoOverview>> SetFavoriteResto(MenuRequest.Resto req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}