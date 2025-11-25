using System.Globalization;
using Rise.Client.Calendar.Fakers;
using Rise.Client.Components;
using Rise.Client.Components.Calendar;
using Rise.Client.Components.Card;
using Rise.Client.Faker;
using Rise.Client.Shared;
using Rise.Shared;
using Rise.Shared.Calendar;
using Shouldly;

namespace Rise.Client.Calendar;

public abstract class GivenACalendarBase<TCalendarComonent> : TestContext
    where TCalendarComonent : CalendarIndex
{
    private readonly DateTime _fixedDate = new(2024, 11, 13);

    protected GivenACalendarBase()
    {
        var culture = CultureInfo.GetCultureInfo("nl-NL");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        var dateTimeServiceMock = new FakeDateTimeService(_fixedDate);
        var pageTitleService = new FakePageTitleService();
        
        Services.AddScoped<IDateTimeService>(_ => dateTimeServiceMock);
        Services.AddScoped<IPageTitleService>(_ => pageTitleService);
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

        var expectedNr = "13"; 
        if (cut.FindComponents<RiseWeekCalendar>().Count > 0)
        {
            expectedNr = "11";
        }

        cut.GetSelectedDayNumber().ShouldBe(expectedNr);
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