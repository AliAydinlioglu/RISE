using Rise.Client.Calendar.Fakers;
using Rise.Client.Components.Card;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.TestDoubles.Fakers;
using Shouldly;

namespace Rise.Client.Calendar;

public abstract class GivenACalendarBase<TCalendarComonent> : TestContext
    where TCalendarComonent : CalendarIndex
{
    private readonly DateTime _fixedDate = new(2024, 11, 13);

    protected GivenACalendarBase()
    {
        var dateTimeServiceMock = new FakeDateTimeService(_fixedDate);
        var pageTitleService = new FakePageTitleService();
        
        Services.AddScoped<IDateTimeService>(_ => dateTimeServiceMock);
        Services.AddScoped<IPageTitleService>(_ => pageTitleService);
    }

    [Fact]
    public void WhenRenderingTheCalendarButItemsNotYetFetched_ThenLoaderShouldBeShown()
    {
        var fakeCalendarService = new FakeCalendarService(true);
        Services.AddScoped<ICalendarService>(_ => fakeCalendarService);

        var cut = RenderComponent<CalendarIndex>();
        cut.Find("p em").TextContent.ShouldBe("Loading...");
    }

    [Fact]
    public void WhenRenderingTheCalendarAfterDataIsLoaded_ThenCalendarShouldBeShown()
    {
        var cut = RenderCalendarComponent();
        cut.FindAll(".box").Count.ShouldBeGreaterThan(0);
    }
    
    [Fact]
    public void WhenCalendarIsLoaded_ThenSelectedDateShouldBeHighlighted()
    {
        var cut = RenderCalendarComponent();

        cut.GetSelectedDayNumber().ShouldBe("13");
    }

    
    [Fact]
    public void WhenSelectedDayHasEvents_ThenEventsShouldBeDisplayed()
    {
        var cut = RenderCalendarComponent();

        var cards = cut.FindComponents<RiseCard>();
        cards.Count.ShouldBeGreaterThan(0);
    }
    
    protected IRenderedComponent<TCalendarComonent> RenderCalendarComponent(bool loading = false)
    {
        var calendarServiceMock = new FakeCalendarService(loading);

        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);
        return RenderComponent<TCalendarComonent>();
    }
}