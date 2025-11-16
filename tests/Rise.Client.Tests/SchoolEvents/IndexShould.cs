using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared.SchoolEvents;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.SchoolEvents;

public class IndexShould : TestContext
{
    private const int PageSize = 8;
    
    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IPageTitleService>(_ => new FakePageTitleService());
    }
    
    [Fact]
    public void ShowLoader_WhenSchoolEventsIsNull()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        
        var cut = RenderComponent<Index>();
        
        cut.Find("[data-bunit='loader']").ShouldNotBeNull(); 
    }

    [Fact]
    public void ShowInfoMessage_WhenNoEvents()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventsForIndex(0);
        
        var cut = RenderComponent<Index>();
        
        cut.Find("[data-bunit='se-no-events-message']")
            .MarkupMatches("<div data-bunit=\"se-no-events-message\" class=\"notification is-light\">" +
                           "<p class=\"has-text-centered\">Geen evenementen voor deze dag.</p>" +
                           "</div>");
    }

    [Fact]
    public void ShowSchoolEvents_WithCorrectAmountAndOnePages()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventsForIndex(6);
        
        var cut = RenderComponent<Index>();
        
        cut.FindAll("[data-bunit='se-index-card']").Count.ShouldBe(6);
        cut.FindAll(".pagination-link").Count.ShouldBe(1);
    }
    
    [Fact]
    public void ShowSchoolEvents_WithCorrectAmountAndMultiplePages()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventsForIndex(10);
        
        var cut = RenderComponent<Index>();
        
        cut.FindAll("[data-bunit='se-index-card']").Count.ShouldBe(8);
        cut.FindAll(".pagination-link").Count.ShouldBe(2);
    }

    [Fact]
    public void ShowSecondPageWithEvents_WhenNextPageIsClicked()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventsForIndex(10);
        
        var cut = RenderComponent<Index>();
        
        cut.Find(".pagination-next").Click();
        cut.Render();
        
        cut.FindAll("[data-bunit='se-index-card']").Count.ShouldBe(2);
    }
    
    [Fact]
    public void ShowBothDesktopAndMobileCards()
    {
        Services.AddScoped<ISchoolEventService, FakeSchoolEventService>();
        var schoolEventService = Services.GetService<ISchoolEventService>() as FakeSchoolEventService;
        schoolEventService!.SetSchoolEventsForIndex(1);

        var cut = RenderComponent<Index>();

        cut.FindAll("[data-bunit='se-index-card'] .is-hidden-mobile").Count.ShouldBe(1);
        cut.FindAll("[data-bunit='se-index-card'] .is-hidden-tablet").Count.ShouldBe(1);
    }
}