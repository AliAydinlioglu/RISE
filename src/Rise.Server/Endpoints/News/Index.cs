using Rise.Shared.Common;
using Rise.Shared.News;

namespace Rise.Server.Endpoints.News;

public class Index(INewsService newsService) : Endpoint<QueryRequest.SkipTake, Result<NewsResponse.Index>>
{
    public override void Configure()
    {
        Get("/api/news");
        AllowAnonymous();
    }

    public override Task<Result<NewsResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return newsService.GetIndexAsync(req, ct);
    }
}
