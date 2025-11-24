using Rise.Shared.Common;

namespace Rise.Shared.Menu;

public interface IMenuService
{
    Task<Result<MenuResponse.DayMenu>> GetDayMenuAsync(MenuRequest.DayMenu req, CancellationToken ct);
}