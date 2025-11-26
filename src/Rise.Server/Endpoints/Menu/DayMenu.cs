using Rise.Shared.Menu;

namespace Rise.Server.Endpoints.Menu;

public class DayMenu
{
    public class Index(IMenuService menuService) : Endpoint<MenuRequest.DayMenu, Result<MenuResponse.DayMenu>>
    {
        public override void Configure()
        {
            Get("/api/menu/day");
            AllowAnonymous();
        }

        public override Task<Result<MenuResponse.DayMenu>> ExecuteAsync(MenuRequest.DayMenu req, CancellationToken ct)
        {
            return menuService.GetDayMenuAsync(req, ct);
        }
    }
}