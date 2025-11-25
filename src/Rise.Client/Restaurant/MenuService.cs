using System.Net.Http.Json;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class MenuService(HttpClient httpClient) : IMenuService
{
    public async Task<Result<MenuResponse.DayMenu>> GetDayMenuAsync(MenuRequest.DayMenu req, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<MenuResponse.DayMenu>>($"/api/menu/day?RestoID={req.RestoId}&Date={req.Date:yyyy-MM-dd}", ctx);
        return result!;
    }
}