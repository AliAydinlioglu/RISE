using MudBlazor.Services;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Xunit.Abstractions;

namespace Rise.Client.Campuses;

public class IndexShould : TestContext
{
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
        Services.AddMudBlazorDialog();
    }

    [Fact]
    public void RenderAllCampusPanels()
    {
        // Act
        var cut = RenderComponent<Index>();

        // Assert: alle panel items aanwezig
        var items = cut.FindAll("[data-bunit='campus-item']");
        Assert.Equal(13, items.Count);  // verwacht 13
    }

    [Fact]
    public void RenderCampusNames()
    {
        var cut = RenderComponent<Index>();

        var names = cut.FindAll("[data-bunit='campus-name']")
                       .Select(el => el.TextContent.Trim())
                       .ToList();

        Assert.Contains("Campus Aalst", names);
        Assert.Contains("Campus Bijloke", names);
        Assert.Contains("Campus Grote Sikkel", names);
    }

    [Fact]
    public void RenderGoogleMapsLink()
    {
        var cut = RenderComponent<Index>();

        var link = cut.Find("a[href*='www.google.com/maps']");

        Assert.Contains("Aalst", link.GetAttribute("href")); // of concreter
    }

    [Fact]
    public void RenderMapsWhenAvailable()
    {
        var cut = RenderComponent<Index>();

        var images = cut.FindAll("img");

        Assert.Contains(images, img => img != null && img!.GetAttribute("src")!.Contains("mercator-plan"));
        Assert.Contains(images, img => img != null && img!.GetAttribute("src")!.Contains("Schoonmeersen"));
    }

    [Fact]
    public void RenderDescriptionsAsHtml()
    {
        var cut = RenderComponent<Index>();

        var html = cut.Find(".campus-desc").InnerHtml;

        Assert.Contains("<p>", html);   // bewijst MarkupString render
        Assert.Contains("Campus Aalst", cut.Markup);
    }
}
