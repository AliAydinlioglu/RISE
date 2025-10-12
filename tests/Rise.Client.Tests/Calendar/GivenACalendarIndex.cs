using Rise.Client.Components;
using Rise.Shared;
using Rise.Shared.Calendar;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Calendar;

public class GivenACalendarIndex : TestContext
{
    private readonly DateTime _fixedDate = new(2024, 11, 13);
    
    public GivenACalendarIndex(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        var dateTimeServiceMock = new FakeDateTimeService(_fixedDate);
        Services.AddScoped<IDateTimeService>(_ => dateTimeServiceMock);
    }
    
    [Fact]
    public void WhenRenderingTheCalendarButItemsNotYetFetched_ThenLoaderShouldBeShown()
    {
        var fakeService = new FakeCalendarService(true);
        Services.AddScoped<ICalendarService>(_ => fakeService);
        
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
    public void WhenRenderingTheCalendarAfterDataIsLoaded_Then5SchooldaysShouldBeVisible()
    {
        var cut = RenderCalendarComponent();

        var dayButtons = cut.FindAll(".columns.is-mobile .column");
        dayButtons.Count.ShouldBe(5);
    }
    
    [Fact]
    public void WhenCalendarIsLoaded_ThenSelectedDateShouldBeHighlighted()
    {
        var cut = RenderCalendarComponent();
        
        var selectedDay = cut.Find(".has-background-white.has-text-black");
        selectedDay.TextContent.Trim().ShouldBe("13");
    }

    [Fact]
    public void WhenClickingPreviousWeek_ThenWeekShouldGoBack()
    {
        var cut = RenderCalendarComponent();

        cut.FindAll(".button.is-ghost")[0].Click();

        var selectedDay = cut.Find(".has-background-white.has-text-black");
        selectedDay.TextContent.Trim().ShouldBe("6");
    }

    [Fact]
    public void WhenClickingNextWeek_ThenWeekShouldAdvance()
    {
        var cut = RenderCalendarComponent();

        cut.FindAll(".button.is-ghost")[1].Click();

        var selectedDay = cut.Find(".has-background-white.has-text-black");
        selectedDay.TextContent.Trim().ShouldBe("20");
    }

    [Fact]
    public void WhenClickingOnADay_ThenThatDayShouldBeSelected()
    {
        var cut = RenderCalendarComponent();

        var dayButtons = cut.FindAll(".column button");
        dayButtons[4].Click();

        var selectedDay = cut.Find(".has-background-white.has-text-black");
        selectedDay.TextContent.Trim().ShouldBe("15");
    }
    
    [Fact]
    public void WhenSelectedDayHasEvents_ThenEventsShouldBeDisplayed()
    {
        var cut = RenderCalendarComponent();

        var cards = cut.FindComponents<Card>();
        cards.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void WhenSelectedDayHasNoEvents_ThenNoEventsMessageShouldBeShown()
    {
        var cut = RenderCalendarComponent();

        var dayButtons = cut.FindAll(".column button");
        dayButtons[4].Click();

        var notification = cut.Find(".notification");
        notification.TextContent.ShouldContain("Geen evenementen voor deze dag");
    }
    
    private IRenderedComponent<CalendarIndex> RenderCalendarComponent()
    {
        var calendarServiceMock = new FakeCalendarService(false);
        
        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);
        
        return RenderComponent<CalendarIndex>();
    }
}