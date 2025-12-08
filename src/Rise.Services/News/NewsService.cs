using Microsoft.EntityFrameworkCore;
using Rise.Domain.News;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.News;

namespace Rise.Services.News;

public class NewsService(ApplicationDbContext dbContext) : INewsService
{
    public async Task<Result<NewsResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var query = dbContext.NewsItems
            .OrderByDescending(n => n.PublishedAt)
            .AsQueryable();

        var totalCount = await query.CountAsync(ctx);

        var newsItems = await query
            .AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(n => ToIndexDto(n))
            .ToListAsync(ctx);

        return Result.Success(new NewsResponse.Index
        {
            NewsItems = newsItems,
            TotalCount = totalCount,
        });
    }

    public async Task<Result<NewsResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
        var newsItem = await dbContext.NewsItems
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id, ctx);

        if (newsItem == null)
            return Result.NotFound($"News item with ID {id} not found.");

        var detail = ToDetailDto(newsItem);

        return Result.Success(new NewsResponse.Detail { NewsItem = detail });
    }

    public static NewsDto.Index ToIndexDto(NewsItem newsItem)
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

    public static NewsDto.Detail ToDetailDto(NewsItem newsItem)
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

