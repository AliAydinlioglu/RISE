using Microsoft.AspNetCore.Components;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Client.Theme;
using Rise.Client.Theme.Fakers;
using Rise.Shared.News;
using Rise.TestDoubles;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.News;

public class DetailShould : TestContext
{
    private const int ValidId = 1;

    public DetailShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
        Services.AddScoped<NavigationManager, FakeNavigationManager>();
        Services.AddScoped<IThemingService, FakeThemingService>();
    }

    [Fact]
    public void NotLoadNewsItem_WhenIdIsNull()
    {
        Services.AddScoped<INewsService, FakeNewsService>();

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, null));

        cut.Find("[data-bunit='news-detail']").ShouldNotBeNull();
    }

    [Fact]
    public void NotLoadNewsItem_WhenIdIsWhitespace()
    {
        Services.AddScoped<INewsService, FakeNewsService>();

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, "   "));

        cut.Find("[data-bunit='news-detail']").ShouldNotBeNull();
    }

    [Fact]
    public void ShowNewsDetailSummary_WhenNewsItemIsLoaded()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateDefaultNewsItem();
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var summary = cut.Find("[data-bunit='news-detail-summary']");
        summary.TextContent.ShouldContain(newsItem.Summary);
    }

    [Fact]
    public void ShowNewsTitle_InHeader()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateDefaultNewsItem();
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var container = cut.Find("[data-bunit='news-detail']");
        container.ToMarkup().ShouldContain(newsItem.Title);
    }

    [Fact]
    public void ShowContentSections_WhenNewsItemHasContent()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateDefaultNewsItem();
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        foreach (var content in newsItem.ContentSections)
        {
            var detailContent = cut.Find("[data-bunit='news-detail']");
            detailContent.ToMarkup().ShouldContain(content);
        }
    }

    [Fact]
    public void DistributeContentAndImages_WithSingleImage()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItem(
            "Test Nieuws",
            "Test samenvatting",
            new List<string> { "Content 1", "Content 2", "Content 3", "Content 4" },
            new List<string> { "/images/test.jpg" },
            DateTime.Now
        );
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        // With 1 image, content should be split: first half in left, image + second half in right
        var leftColumn = cut.Find("[data-bunit='news-detail-content-left']");
        var rightColumn = cut.Find("[data-bunit='news-detail-content-right']");

        leftColumn.ShouldNotBeNull();
        rightColumn.ShouldNotBeNull();
        rightColumn.ToMarkup().ShouldContain("/images/test.jpg");
    }

    [Fact]
    public void DistributeContentAndImages_WithMultipleImages()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItemWithMultipleImages(4);
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var leftColumn = cut.Find("[data-bunit='news-detail-content-left']");
        var rightColumn = cut.Find("[data-bunit='news-detail-content-right']");

        // Both columns should have content and images distributed
        leftColumn.ShouldNotBeNull();
        rightColumn.ShouldNotBeNull();

        // Verify images are present
        var images = cut.FindAll(".news-detail-image-item img");
        images.Count.ShouldBe(4);
    }

    [Fact]
    public void ShowContentWithoutImages_WhenNoImagesProvided()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItemWithNoImages();
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var leftColumn = cut.Find("[data-bunit='news-detail-content-left']");
        var rightColumn = cut.Find("[data-bunit='news-detail-content-right']");

        leftColumn.ShouldNotBeNull();
        rightColumn.ShouldNotBeNull();

        // Verify no images are rendered
        var images = cut.FindAll(".news-detail-image-item");
        images.Count.ShouldBe(0);
    }

    [Fact]
    public void ShowBackButton_WithCorrectNavigationLink()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemForDetail(NewsTestDataFactory.CreateDefaultNewsItem());

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var backButton = cut.Find("a[href='/news']");
        backButton.ShouldNotBeNull();
        backButton.TextContent.ShouldContain("Terug");
    }

    [Fact]
    public void ParseIdCorrectly_WhenValidIdProvided()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemForDetail(NewsTestDataFactory.CreateDefaultNewsItem());

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, "123"));

        // Verify component renders successfully, meaning ID was parsed correctly
        cut.Find("[data-bunit='news-detail']").ShouldNotBeNull();
    }

    [Fact]
    public void ShowNewsItemInLeftColumn_WithCorrectContent()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItem(
            "Test Nieuws",
            "Test samenvatting",
            new List<string> { "Left content 1", "Left content 2", "Right content 1" },
            new List<string> { "/images/test.jpg" },
            DateTime.Now
        );
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var leftColumn = cut.Find("[data-bunit='news-detail-content-left']");
        leftColumn.TextContent.ShouldContain("Left content");
    }

    [Fact]
    public void ShowNewsItemInRightColumn_WithCorrectContent()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItem(
            "Test Nieuws",
            "Test samenvatting",
            new List<string> { "Left content 1", "Left content 2", "Right content 1" },
            new List<string> { "/images/test.jpg" },
            DateTime.Now
        );
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var rightColumn = cut.Find("[data-bunit='news-detail-content-right']");
        rightColumn.TextContent.ShouldContain("Right content");
    }

    [Fact]
    public void ShowImages_WithCorrectAltText()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateDefaultNewsItem();
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var images = cut.FindAll(".news-detail-image-item img");
        foreach (var image in images)
        {
            image.GetAttribute("alt").ShouldBe("News image");
        }
    }

    [Fact]
    public void RenderHTML_InContentSections()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        var newsItem = NewsTestDataFactory.CreateNewsItem(
            "Test Nieuws",
            "Test samenvatting",
            new List<string> { "<strong>Bold content</strong>", "<em>Italic content</em>" },
            null,
            DateTime.Now
        );
        newsService!.SetNewsItemForDetail(newsItem);

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var detailContent = cut.Find("[data-bunit='news-detail']");
        detailContent.ToMarkup().ShouldContain("<strong>Bold content</strong>");
        detailContent.ToMarkup().ShouldContain("<em>Italic content</em>");
    }

    [Fact]
    public void ShowBothDesktopAndMobileHeaders()
    {
        Services.AddScoped<INewsService, FakeNewsService>();
        var newsService = Services.GetService<INewsService>() as FakeNewsService;
        newsService!.SetNewsItemForDetail(NewsTestDataFactory.CreateDefaultNewsItem());

        var cut = RenderComponent<Detail>(p => p.Add(x => x.Id, ValidId.ToString()));

        var desktopHeader = cut.FindAll(".is-hidden-mobile");
        var mobileHeader = cut.FindAll(".is-hidden-tablet");

        desktopHeader.Count.ShouldBeGreaterThan(0);
        mobileHeader.Count.ShouldBeGreaterThan(0);
    }
}

