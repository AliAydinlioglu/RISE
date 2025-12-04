using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared.News;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.News;

public class IndexShould : TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
    }

    [Fact]
    public void ShowNewsItems_WithCorrectAmountOnOnePage()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(5);

        var cut = RenderComponent<Index>();

        cut.FindAll("[data-bunit='news-index']").Count.ShouldBe(5);
        cut.FindAll(".pagination-link").Count.ShouldBe(1);
    }

    [Fact]
    public void ShowNewsItems_WithCorrectAmountAndMultiplePages()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(20);

        var cut = RenderComponent<Index>();

        cut.FindAll("[data-bunit='news-index']").Count.ShouldBe(8);
        cut.FindAll(".pagination-link").Count.ShouldBe(3);
    }

    [Fact]
    public void ShowCorrectNewsItemDetails_WhenNewsItemsAreLoaded()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(3);
        var newsItems = newsService.GetNewsItemsForIndex().ToList();

        var cut = RenderComponent<Index>();

        var newsArticles = cut.FindAll("[data-bunit='news-index']");
        newsArticles.Count.ShouldBe(3);

        // Test first news item
        var firstArticle = newsArticles[0];
        firstArticle.ToMarkup().ShouldContain(newsItems[0].Title);
        firstArticle.ToMarkup().ShouldContain(newsItems[0].Summary);
        firstArticle.ToMarkup().ShouldContain(newsItems[0].PublishedAt.ToString("dd/MM/yyyy"));
    }

    [Fact]
    public void ShowSecondPageWithCorrectItems_WhenNextPageIsClicked()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(10);

        var cut = RenderComponent<Index>();

        // Verify first page has 8 items
        cut.FindAll("[data-bunit='news-index']").Count.ShouldBe(8);

        // Click next page
        cut.Find(".pagination-next").Click();
        cut.Render();

        // Verify second page has 2 items (10 total - 8 on first page)
        cut.FindAll("[data-bunit='news-index']").Count.ShouldBe(2);
    }

    [Fact]
    public void DisablePreviousButton_WhenOnFirstPage()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(10);

        var cut = RenderComponent<Index>();

        var previousButton = cut.Find(".pagination-previous");
        previousButton.HasAttribute("disabled").ShouldBeTrue();
    }

    [Fact]
    public void DisableNextButton_WhenOnLastPage()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(10);

        var cut = RenderComponent<Index>();

        // Navigate to last page
        cut.Find(".pagination-next").Click();
        cut.Render();

        var nextButton = cut.Find(".pagination-next");
        nextButton.HasAttribute("disabled").ShouldBeTrue();
    }

    [Fact]
    public void NavigateToSpecificPage_WhenPageNumberIsClicked()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(20);

        var cut = RenderComponent<Index>();

        // Click on page 2
        var pageButtons = cut.FindAll(".pagination-link");
        pageButtons[1].Click(); // Index 1 is page 2
        cut.Render();

        // Verify page 2 is active
        var activeButton = cut.Find(".pagination-link.is-current");
        activeButton.TextContent.Trim().ShouldBe("2");
    }

    [Fact]
    public void ShowBackToPreviousPage_WhenPreviousButtonIsClicked()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(20);

        var cut = RenderComponent<Index>();

        // Navigate to page 2
        cut.Find(".pagination-next").Click();
        cut.Render();

        // Verify we're on page 2
        var activeButton = cut.Find(".pagination-link.is-current");
        activeButton.TextContent.Trim().ShouldBe("2");

        // Go back to page 1
        cut.Find(".pagination-previous").Click();
        cut.Render();

        // Verify we're back on page 1
        activeButton = cut.Find(".pagination-link.is-current");
        activeButton.TextContent.Trim().ShouldBe("1");
    }

    [Fact]
    public void ShowNewsItemWithImage_WhenImageUrlExists()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(1);
        var newsItems = newsService.GetNewsItemsForIndex().ToList();
        var imageUrl = newsItems[0].ImageUrls.FirstOrDefault();

        var cut = RenderComponent<Index>();

        var newsArticle = cut.Find("[data-bunit='news-index']");
        newsArticle.ToMarkup().ShouldContain($"background-image: url('{imageUrl}')");
    }

    [Fact]
    public void ShowNewsItemLink_WithCorrectHref()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(1);
        var newsItems = newsService.GetNewsItemsForIndex().ToList();

        var cut = RenderComponent<Index>();

        var link = cut.Find("a.news-item-link");
        link.GetAttribute("href").ShouldBe($"/news/{newsItems[0].Id}");
    }

    [Fact]
    public void ShowCorrectTotalPagesCount()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(25); // Should result in 4 pages (25 / 8 = 3.125, rounded up to 4)

        var cut = RenderComponent<Index>();

        cut.FindAll(".pagination-link").Count.ShouldBe(4);
    }

    [Fact]
    public void ShowMoreDetailsLink_OnEachNewsItem()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemsForIndex(3);

        var cut = RenderComponent<Index>();

        var moreDetailsLinks = cut.FindAll(".news-item-more-details");
        moreDetailsLinks.Count.ShouldBe(3);
        foreach (var link in moreDetailsLinks)
        {
            link.TextContent.ShouldContain("Meer details");
        }
    }
}

