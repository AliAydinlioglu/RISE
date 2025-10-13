using Rise.Client.Calendar.Fakers;
using Rise.Client.Components;
using Rise.Shared;
using Rise.Shared.Calendar;
using Rise.TestDoubles.Fakers;
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

        cut.GetDayColumnsCount().ShouldBe(5);
    }
    
    [Fact]
    public void WhenCalendarIsLoaded_ThenSelectedDateShouldBeHighlighted()
    {
        var cut = RenderCalendarComponent();
        
        cut.GetSelectedDayNumber().ShouldBe("13");
    }

    [Fact]
    public void WhenClickingPreviousWeek_ThenWeekShouldGoBack()
    {
        var cut = RenderCalendarComponent();

        cut.NavigateToPreviousWeek();

        cut.GetSelectedDayNumber().ShouldBe("6");
    }

    [Fact]
    public void WhenClickingNextWeek_ThenWeekShouldAdvance()
    {
        var cut = RenderCalendarComponent();

        cut.NavigateToNextWeek();

        cut.GetSelectedDayNumber().ShouldBe("20");
    }

    [Fact]
    public void WhenClickingOnADay_ThenThatDayShouldBeSelected()
    {
        var cut = RenderCalendarComponent();

        cut.ClickDay(4);

        cut.GetSelectedDayNumber().ShouldBe("15");
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

        cut.ClickDay(4);

        var notification = cut.Find(".notification");
        notification.TextContent.ShouldContain("Geen evenementen voor deze dag");
    }
    
    [Fact]
    public void WhenCalendarIsLoaded_ThenTitleShouldBeKalender()
    {
        var cut = RenderCalendarComponent();

        cut.GetTitle().ShouldBe("Kalender");
    }
    
    [Fact]
    public void WhenCarouselIsLoaded_Then1DotShouldBeShownForEveryTab()
    {
        var cut = RenderCalendarComponent();

        cut.GetDots().Count.ShouldBe(3);
    }
    
    [Fact]
    public void WhenCarouselIsLoaded_ThenFirstDotShouldBeActive()
    {
        var cut = RenderCalendarComponent();
        
        cut.IsDotActive(CalendarView.Kalender).ShouldBeTrue();
    }
    
    [Fact]
    public void WhenSwitchingBetweenViews_ThenTitleAndActiveDotShouldUpdateAccordingly()
    {
        var cut = RenderCalendarComponent();
        
        // Initial state: Kalender
        cut.GetTitle().ShouldBe("Kalender");
        cut.IsDotActive(CalendarView.Kalender).ShouldBeTrue();
        cut.IsDotActive(CalendarView.Lessenrooster).ShouldBeFalse();
        cut.IsDotActive(CalendarView.Deadlines).ShouldBeFalse();

        // Ga naar Lessenrooster
        cut.SwitchToView(CalendarView.Lessenrooster);
        cut.GetTitle().ShouldBe("Lessenrooster");
        cut.IsDotActive(CalendarView.Kalender).ShouldBeFalse();
        cut.IsDotActive(CalendarView.Lessenrooster).ShouldBeTrue();
        cut.IsDotActive(CalendarView.Deadlines).ShouldBeFalse();

        // Ga naar Deadlines
        cut.SwitchToView(CalendarView.Deadlines);
        cut.GetTitle().ShouldBe("Deadlines");
        cut.IsDotActive(CalendarView.Kalender).ShouldBeFalse();
        cut.IsDotActive(CalendarView.Lessenrooster).ShouldBeFalse();
        cut.IsDotActive(CalendarView.Deadlines).ShouldBeTrue();

        // Ga terug naar Kalender
        cut.SwitchToView(CalendarView.Kalender);
        cut.GetTitle().ShouldBe("Kalender");
        cut.IsDotActive(CalendarView.Kalender).ShouldBeTrue();
        cut.IsDotActive(CalendarView.Lessenrooster).ShouldBeFalse();
        cut.IsDotActive(CalendarView.Deadlines).ShouldBeFalse();
    }
    
    [Fact]
    public void WhenOnDeadlinesView_ThenWeekHeaderShouldNotBeVisible()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Deadlines);

        cut.GetWeekHeaderCount().ShouldBe(0);
    }
    
    [Fact]
    public void WhenOnLessenroosterView_ThenOnlyCoursesForSelectedDateShouldBeShown()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Lessenrooster);

        cut.GetCardsInVisibleView().Length.ShouldBe(1);
    }
    
    [Fact]
    public void WhenOnLessenroosterViewAndNoDayHasLessons_ThenEmptyMessageShouldBeShown()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Lessenrooster);
        cut.ClickDay(4);

        var notification = cut.GetNotificationInVisibleView();
        notification?.TextContent.ShouldContain("Geen lessen voor deze dag");
    }
    
    [Fact]
    public void WhenOnDeadlinesView_ThenAllDeadlinesShouldBeShownOrderedByDate()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Deadlines);

        var visibleItem = cut.GetVisibleCarouselItem();
        var deadlineCards = visibleItem.QuerySelectorAll(".box");
        deadlineCards.Length.ShouldBe(1);
    }
    
    [Fact]
    public void WhenOnDeadlinesViewAndNoDeadlinesExist_ThenEmptyMessageShouldBeShown()
    {
        var calendarServiceMock = new FakeCalendarServiceWithoutDeadlines();
        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);
    
        var cut = RenderComponent<CalendarIndex>();

        cut.SwitchToView(CalendarView.Deadlines);

        var notification = cut.Find(".notification");
        notification.TextContent.ShouldContain("Geen deadlines beschikbaar");
    }
    
    private IRenderedComponent<CalendarIndex> RenderCalendarComponent()
    {
        var calendarServiceMock = new FakeCalendarService(false);
        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);
        
        return RenderComponent<CalendarIndex>();
    }
}