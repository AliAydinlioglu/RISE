using Rise.Client.Calendar.Components;
using Rise.Client.Calendar.Fakers;
using Rise.Shared.Calendar;
using Shouldly;

namespace Rise.Client.Calendar;

public class GivenADesktopCalendar : GivenACalendarBase<CalendarDesktop>
{
    [Fact]
    public void WhenCalendarIsLoaded_ThenAllSectionsShouldBeVisible()
    {
        var cut = RenderCalendarComponent();

        var boxes = cut.FindAll(".column");
        boxes.Count.ShouldBe(4);
    }

    [Fact]
    public void WhenCalendarIsLoaded_ThenMonthCalendarShouldBeVisible()
    {
        var cut = RenderCalendarComponent();

        var monthCalendar = cut.FindComponent<MonthCalendar>();
        monthCalendar.ShouldNotBeNull();
    }

    [Fact]
    public void WhenCalendarIsLoaded_ThenLessenroosterSectionShouldShowCoursesForSelectedDate()
    {
        var cut = RenderCalendarComponent();

        var lessenroosterBox = cut.GetBox(CalendarView.Lessenrooster);
        lessenroosterBox.QuerySelector("h2")?.TextContent.ShouldBe("Lessenrooster");
    }

    [Fact]
    public void WhenCalendarIsLoaded_ThenDeadlinesSectionShouldShowAllDeadlines()
    {
        var cut = RenderCalendarComponent();

        var deadlinesBox = cut.GetBox(CalendarView.Deadlines);
        deadlinesBox.QuerySelector("h2")?.TextContent.ShouldBe("Deadlines");
    }

    [Fact]
    public void WhenCalendarIsLoaded_ThenEvenementenSectionShouldBeVisible()
    {
        var cut = RenderCalendarComponent();

        var evenementenBox = cut.GetBox(CalendarView.Evenementen);
        evenementenBox.QuerySelector("h2")?.TextContent.ShouldBe("Evenementen");
    }

    [Fact]
    public void WhenNoCoursesForSelectedDate_ThenLessenroosterShouldShowEmptyMessage()
    {
        var cut = RenderCalendarComponent();
        cut.ClickDay(2); // Day with no lesson

        var lessenroosterBox = cut.GetBox(CalendarView.Lessenrooster);
        var notification = lessenroosterBox.QuerySelector(".notification");
        notification!.TextContent.ShouldContain("Geen lessen voor deze dag");
    }

    [Fact]
    public void WhenNoDeadlinesExist_ThenDeadlinesSectionShouldShowEmptyMessage()
    {
        var calendarServiceMock = new FakeCalendarServiceWithoutDeadlines();
        Services.AddScoped<ICalendarService>(_ => calendarServiceMock);

        var cut = RenderComponent<CalendarDesktop>();

        var deadlinesBox = cut.GetBox(CalendarView.Deadlines);
        var notification = deadlinesBox.QuerySelector(".notification");
        notification!.TextContent.ShouldContain("Geen deadlines beschikbaar");
    }
    
    [Fact]
    public void WhenClickingNextMonth_ThenMonthShouldAdvanceByOne()
    {
        var cut = RenderCalendarComponent();
        var initialMonth = cut.Find("h2.title").TextContent;
        initialMonth.ShouldBe("november");

        var nextButton = cut.Find(".is-flex").QuerySelectorAll("button")[1];
        nextButton.Click();

        var newMonth = cut.Find("h2.title").TextContent;
        newMonth.ShouldBe("december");
    }
    
    [Fact]
    public void WhenClickingPreviousMonth_ThenMonthShouldRewindByOne()
    {
        var cut = RenderCalendarComponent();
        var initialMonth = cut.Find("h2.title").TextContent;
        initialMonth.ShouldBe("november");

        var prevButton = cut.Find(".is-flex").QuerySelectorAll("button")[0];
        prevButton.Click();

        var newMonth = cut.Find("h2.title").TextContent;
        newMonth.ShouldBe("oktober");
    }
    
}