using Rise.Shared.Common;

namespace Rise.Shared.News;

public interface INewsService
{
    Task<Result<NewsResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default);
    
    Task<Result<NewsResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default);
}

