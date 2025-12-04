using Ardalis.Result;
using Rise.Domain.News;
using Rise.Shared.Common;
using Rise.Shared.News;
using Rise.TestDoubles;

namespace Rise.Client.News;

public class FakeNewsService : INewsService
{
    private IEnumerable<NewsItem>? _newsItemsForIndex;
    private NewsItem? _newsItemForDetail;

    public void SetNewsItemsForIndex(int count)
    {
        _newsItemsForIndex = NewsTestDataFactory.CreateTestNewsItems(count);
    }

    public IEnumerable<NewsItem> GetNewsItemsForIndex()
    {
        return _newsItemsForIndex ?? [];
    }

    public void SetNewsItemForDetail(NewsItem newsItem)
    {
        _newsItemForDetail = newsItem;
    }

    public NewsItem? GetNewsItemForDetail()
    {
        return _newsItemForDetail;
    }

    public Task<Result<NewsResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var items = _newsItemsForIndex?
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(ToIndexDto)
            .ToList() ?? new List<NewsDto.Index>();

        var result = new NewsResponse.Index
        {
            NewsItems = items,
            TotalCount = _newsItemsForIndex?.Count() ?? 0
        };

        return Task.FromResult(Result.Success(result));
    }
    
    private static NewsDto.Index ToIndexDto(NewsItem newsItem)
    {
        return new NewsDto.Index
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Summary = newsItem.Summary,
            ImageUrls = newsItem.ImageUrls,
            PublishedAt = newsItem.PublishedAt
        };
    }

    public Task<Result<NewsResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
        var result = new NewsResponse.Detail
        {
            NewsItem = _newsItemForDetail != null ? ToDetailDto(_newsItemForDetail) : null!
        };

        return Task.FromResult(Result.Success(result));
    }
    
    private static NewsDto.Detail ToDetailDto(NewsItem newsItem)
    {
        return new NewsDto.Detail
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Summary = newsItem.Summary,
            ContentSections = newsItem.ContentSections,
            ImageUrls = newsItem.ImageUrls,
            PublishedAt = newsItem.PublishedAt
        };
    }
}

