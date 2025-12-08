using Rise.Shared.News;

namespace Rise.Server.Endpoints.News;

public class Detail(INewsService newsService) : Endpoint<NewsRequest.Detail, Result<NewsResponse.Detail>>
{
    public override void Configure()
    {
        Get("/api/news/{Id}");
        AllowAnonymous();
    }

    public override Task<Result<NewsResponse.Detail>> ExecuteAsync(NewsRequest.Detail req, CancellationToken ctx)
    {
        return newsService.GetDetailByIdAsync(req.Id, ctx);
    }
}

