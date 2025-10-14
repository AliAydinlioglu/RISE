using Rise.Client.Calendar.Components;
using Rise.Client.Calendar.Fakers;
using Rise.Shared.Calendar;
using Shouldly;

namespace Rise.Client.Calendar;

public class GivenAMobileCalendar : GivenACalendarBase<CalendarMobile>
{
    
    [Fact]
    public void WhenCalendarIsLoaded_Then5SchooldaysShouldBeVisible()
    {
        var cut = RenderCalendarComponent();

        var visibleItem = cut.GetVisibleCarouselItem();
        visibleItem.QuerySelectorAll(".columns.is-mobile .column").Length.ShouldBe(5);
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
    public void WhenSwitchingBetweenWeeksAndDays_ThenAClickedDayShouldBeSelected()
    {
        var cut = RenderCalendarComponent();

        cut.ClickDay(4);

        cut.GetSelectedDayNumber().ShouldBe("15");
    }

    [Fact]
    public void WhenSwitchingBetweenWeeksAndDays_ThenAdvancingAWeekShouldSetSelectedDayToMondayOfNextWeek()
    {
        var cut = RenderCalendarComponent();

        cut.FindAll(".button.is-ghost")[1].Click();

        cut.GetSelectedDayNumber().ShouldBe("18");
    }

    [Fact]
    public void WhenSwitchingBetweenWeeksAndDays_ThenRewindingAWeekShouldSetSelectedDayToMondayOfPreviousWeek()
    {
        var cut = RenderCalendarComponent();

        cut.FindAll(".button.is-ghost")[0].Click();

        cut.GetSelectedDayNumber().ShouldBe("4");
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
    public void WhenOnDeadlinesView_ThenWeekHeaderShouldNotBeVisible()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Deadlines);

        var visibleItem = cut.GetVisibleCarouselItem();
        visibleItem.QuerySelectorAll(".box .mb-5").Length.ShouldBe(0);
    }

    [Fact]
    public void WhenOnDeadlinesView_ThenEmptyMessageShouldBeShownIfNoDeadlinesAreAvailable()
    {
        var calendarServiceMock = new FakeCalendarServiceWithoutDeadlines();
        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);

        var cut = RenderComponent<CalendarMobile>();

        cut.SwitchToView(CalendarView.Deadlines);

        var notification = cut.Find(".notification");
        notification.TextContent.ShouldContain("Geen deadlines beschikbaar");
    }

    [Fact]
    public void WhenOnLessenroosterView_ThenOnlyCoursesForSelectedDateShouldBeShown()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Lessenrooster);

        var visibleItem = cut.GetVisibleCarouselItem();
        visibleItem.QuerySelectorAll(".p-0").Length.ShouldBe(1);
    }

    [Fact]
    public void WhenOnLessenroosterView_ThenEmptyMessageShouldBeShownIfLessonsAreNotAvailableForSelectedDate()
    {
        var cut = RenderCalendarComponent();

        cut.SwitchToView(CalendarView.Lessenrooster);
        cut.ClickDay(4);

        var visibleItem = cut.GetVisibleCarouselItem();
        var notification = visibleItem.QuerySelector(".notification");
        notification!.TextContent.ShouldContain("Geen lessen voor deze dag");
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
    public void WhenSelectedDayHasNoEvents_ThenNoEventsMessageShouldBeShown()
    {
        var cut = RenderCalendarComponent();

        cut.ClickDay(4);

        var notification = cut.Find(".notification");
        notification.TextContent.ShouldContain("Geen evenementen voor deze dag");
    }
}