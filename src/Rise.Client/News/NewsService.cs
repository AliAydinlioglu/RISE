using Rise.Shared.Common;
using Rise.Shared.News;
using System.Net.Http.Json;

namespace Rise.Client.News;

public class NewsService(HttpClient httpClient) : INewsService
{
    public async Task<Result<NewsResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var result = await httpClient.GetFromJsonAsync<Result<NewsResponse.Index>>($"/api/news?skip={request.Skip}&take={request.Take}", cancellationToken: ctx);
        return result!;
    }

    public async Task<Result<NewsResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default)
    {
        var result = await httpClient.GetFromJsonAsync<Result<NewsResponse.Detail>>($"/api/news/{id}", cancellationToken: ctx);
        return result!;
    }
}

