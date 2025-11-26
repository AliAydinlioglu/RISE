using Rise.Client.Components.Calendar;
using Shouldly;
using Xunit.Abstractions;

namespace Rise.Client.Components.CalendarComponents;

public class GivenAWeekCalendar : TestContext
{
    public GivenAWeekCalendar(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
    }
    
    [Fact]
    public void WhenShowOnlySchoolDaysTrue_ThenRenderCorrectNumberOfDays()
    {
        // arrange
        var selectedDate = new DateTime(2025, 11, 12); 
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.SelectedDate, selectedDate)
            .Add(p => p.ShowOnlySchoolDays, true)
        );

        // act
        var dayButtons = cut.FindAll("[data-bunit='calendar-day']").Where(b => int.TryParse(b.TextContent, out _)).ToList();

        // assert
        dayButtons.Count.ShouldBe(5); // schooldays only
    }
    
    [Fact]
    public void WhenShowOnlySchoolDaysFalse_ThenRenderCorrectNumberOfDays()
    {
        // arrange
        var selectedDate = new DateTime(2025, 11, 12);
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.SelectedDate, selectedDate)
            .Add(p => p.ShowOnlySchoolDays, false)
        );

        // act
        var dayButtons = cut.FindAll("[data-bunit='calendar-day']").Where(b => int.TryParse(b.TextContent, out _)).ToList();
        
        // assert
        dayButtons.Count.ShouldBe(7); // full week
    }
    
    [Fact]
    public void WhenSelectingDate_ThenSelectedDateUpdates()
    {
        // arrange
        DateTime? selectedDate = null;
        var initialDate = new DateTime(2025, 11, 12);
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.SelectedDate, initialDate)
            .Add(p => p.SelectedDateChanged, date => { selectedDate = date; })
            .Add(p => p.CalendarType, RiseWeekCalendar.RiseWeekCalendarType.Primary)
        );

        // act
        var dayToSelect = initialDate.AddDays(1);
        var buttonToSelect = cut.FindAll("[data-bunit='calendar-day']").First(b => b.TextContent.Contains(dayToSelect.Day.ToString()));
        buttonToSelect.Click();

        // assert 
        selectedDate.ShouldBe(dayToSelect);
        
        buttonToSelect.ClassList.ShouldContain("has-text-white"); 
    }
    
    [Fact]
    public void WhenClickingNextWeek_ThenNextWeekIsShown()
    {
        // assign
        var nextCalled = false;
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.OnNextWeek, () => { nextCalled = true; return Task.CompletedTask; })
        );

        // act
        cut.Find("button .fa-chevron-right").Click();
        
        // assert
        nextCalled.ShouldBeTrue();
    }
    
    [Fact]
    public void WhenClickingPreviousWeek_ThenPreviousWeekIsShown()
    {
        // assign
        var previousCalled = false;
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.OnPreviousWeek, () => { previousCalled = true; return Task.CompletedTask; })
        );

        // act
        cut.Find("button .fa-chevron-left").Click();
        
        // assert
        previousCalled.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(RiseWeekCalendar.RiseWeekCalendarType.Primary, "has-background-black", "has-text-white")]
    [InlineData(RiseWeekCalendar.RiseWeekCalendarType.Secondary, "has-background-white", "has-text-black")]
    public void WhenCalenderTypeIsChosen_ThenCorrectCalendarTypeClassesAreRendered(RiseWeekCalendar.RiseWeekCalendarType type, string bgClass, string fgClass)
    {
        // assign
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.SelectedDate, DateTime.Today)
            .Add(p => p.CalendarType, type)
        );

        // assert
        var rootDiv = cut.Find("div.box");
        rootDiv.ClassList.ShouldContain(bgClass);

        var title = cut.Find("p.title");
        title.ClassList.ShouldContain(fgClass);
    }
    
    [Theory]
    [InlineData(RiseWeekCalendar.RiseWeekCalendarWidth.Small, "rise-calendar-width-small")]
    [InlineData(RiseWeekCalendar.RiseWeekCalendarWidth.Medium, "rise-calendar-width-medium")]
    [InlineData(RiseWeekCalendar.RiseWeekCalendarWidth.Large, "rise-calendar-width-large")]
    public void WhenWidthIsChosen_ThenCorrectWidthClassIsRendered(RiseWeekCalendar.RiseWeekCalendarWidth width, string expectedClass)
    {
        var cut = RenderComponent<RiseWeekCalendar>(parameters => parameters
            .Add(p => p.SelectedDate, DateTime.Today)
            .Add(p => p.CalendarWidth, width)
        );

        var rootDiv = cut.Find("div.box");
        rootDiv.ClassList.ShouldContain(expectedClass);
    }
}