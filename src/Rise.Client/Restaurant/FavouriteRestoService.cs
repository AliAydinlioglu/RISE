using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Rise.Shared.Menu;

namespace Rise.Client.Restaurant;

public class FavouriteRestoService : IFavouriteRestoService
{
    private readonly IJSRuntime _js;
    private const string KEY = "favouriteResto";

    public FavouriteRestoService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetFavouriteRestoAsync(RestoOverviewDto resto)
    {
        var json = JsonSerializer.Serialize(resto);
        await _js.InvokeVoidAsync("localStorage.setItem", KEY, json);
    }

    public async Task<RestoOverviewDto?> GetFavouriteRestoAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", KEY);
        if (string.IsNullOrWhiteSpace(json))
            return null;
        return JsonSerializer.Deserialize<RestoOverviewDto>(json);
    }

}

public interface IFavouriteRestoService
{
    Task SetFavouriteRestoAsync(RestoOverviewDto resto);
    Task<RestoOverviewDto?> GetFavouriteRestoAsync();
}