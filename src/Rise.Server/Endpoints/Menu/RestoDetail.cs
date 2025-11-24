using Rise.Shared.Menu;

namespace Rise.Server.Endpoints.Menu;

public class RestoDetail
{
    public class Index(IRestoService restoService) : Endpoint<MenuRequest.Resto, Result<MenuResponse.RestoDetail>>
    {
        public override void Configure()
        {
            Get("/api/menu/restos");
            AllowAnonymous();
        }

        public override Task<Result<MenuResponse.RestoDetail>> ExecuteAsync(MenuRequest.Resto req, CancellationToken ct)
        {
            return restoService.GetDetailAsync(req, ct);
        }
    }
}