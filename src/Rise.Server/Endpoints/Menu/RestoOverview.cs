using Rise.Shared.Common;
using Rise.Shared.Menu;

namespace Rise.Server.Endpoints.Menu;

public class RestoOverview
{
    public class Index(IRestoService restoService) : Endpoint<QueryRequest.SkipTake, Result<MenuResponse.RestoOverview>>
    {
        public override void Configure()
        {
            Get("/api/menu/restos");
            AllowAnonymous();
        }

        public override Task<Result<MenuResponse.RestoOverview>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
        {
            return restoService.GetOverviewAsync(req, ct);
        }
    }
}