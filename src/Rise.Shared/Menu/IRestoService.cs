using Rise.Shared.Common;

namespace Rise.Shared.Menu;

public interface IRestoService
{
    Task<Result<MenuResponse.RestoOverview>> GetOverviewAsync(QueryRequest.SkipTake req, CancellationToken ct);
    Task<Result<MenuResponse.RestoDetail>> GetDetailAsync(MenuRequest.Resto req, CancellationToken ct);
    Task<Result<MenuResponse.RestoOverview>> SetFavoriteResto(MenuRequest.Resto req, CancellationToken ct);
}