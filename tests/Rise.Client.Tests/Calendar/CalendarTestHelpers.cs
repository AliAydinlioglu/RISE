using AngleSharp.Dom;
using Rise.Client.Components;

namespace Rise.Client.Calendar;

public static class CalendarTestHelpers
{
    public static IElement GetVisibleCarouselItem(this IRenderedComponent<CalendarIndex> component)
    {
        var carousel = component.FindComponent<RiseCarousel>();
        return carousel.Find(".carousel-item[style*='display: block']");
    }

    public static IRefreshableElementCollection<IElement> GetDots(this IRenderedComponent<CalendarIndex> component)
    {
        return component.FindAll(".dot");
    }

    public static void SwitchToView(this IRenderedComponent<CalendarIndex> component, CalendarView view)
    {
        var dots = component.GetDots();
        dots[(int)view].Click();
    }

    public static string GetTitle(this IRenderedComponent<CalendarIndex> component)
    {
        return component.Find("h3.mud-typography-h3").TextContent;
    }

    public static string GetSelectedDayNumber(this IRenderedComponent<CalendarIndex> component)
    {
        return component.Find(".has-background-white.has-text-black").TextContent.Trim();
    }

    public static void ClickDay(this IRenderedComponent<CalendarIndex> component, int dayIndex)
    {
        var dayButtons = component.FindAll(".column button");
        dayButtons[dayIndex].Click();
    }

    public static bool IsDotActive(this IRenderedComponent<CalendarIndex> component, CalendarView view)
    {
        var dots = component.GetDots();
        return dots[(int)view].ClassList.Contains("is-active");
    }
    
    public static IElement GetBox(this IRenderedComponent<CalendarIndex> component, CalendarView view)
    {
        return component.FindAll(".column")[(int)view].Children[0];
    }
}

public enum CalendarView
{
    Kalender = 0,
    Lessenrooster = 1,
    Deadlines = 2,
    Evenementen = 3
}