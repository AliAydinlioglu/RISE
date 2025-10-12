using AngleSharp.Dom;
using Rise.Client.Components;

namespace Rise.Client.Calendar;

public static class CalendarTestHelpers
{
    public static IElement GetVisibleCarouselItem(this IRenderedComponent<CalendarIndex> component)
    {
        var carousel = component.FindComponent<Carousel>();
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
        return component.Find("h1.title").TextContent;
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

    public static void NavigateToPreviousWeek(this IRenderedComponent<CalendarIndex> component)
    {
        component.FindAll(".button.is-ghost")[0].Click();
    }

    public static void NavigateToNextWeek(this IRenderedComponent<CalendarIndex> component)
    {
        component.FindAll(".button.is-ghost")[1].Click();
    }

    public static IHtmlCollection<IElement> GetCardsInVisibleView(this IRenderedComponent<CalendarIndex> component)
    {
        var visibleItem = component.GetVisibleCarouselItem();
        return visibleItem.QuerySelectorAll(".p-0");
    }

    public static IElement? GetNotificationInVisibleView(this IRenderedComponent<CalendarIndex> component)
    {
        var visibleItem = component.GetVisibleCarouselItem();
        return visibleItem.QuerySelector(".notification");
    }

    public static int GetWeekHeaderCount(this IRenderedComponent<CalendarIndex> component)
    {
        var visibleItem = component.GetVisibleCarouselItem();
        return visibleItem.QuerySelectorAll(".box .mb-5").Length;
    }

    public static int GetDayColumnsCount(this IRenderedComponent<CalendarIndex> component)
    {
        var visibleItem = component.GetVisibleCarouselItem();
        return visibleItem.QuerySelectorAll(".columns.is-mobile .column").Length;
    }

    public static bool IsDotActive(this IRenderedComponent<CalendarIndex> component, CalendarView view)
    {
        var dots = component.GetDots();
        return dots[(int)view].ClassList.Contains("is-active");
    }
}

public enum CalendarView
{
    Kalender = 0,
    Lessenrooster = 1,
    Deadlines = 2
}