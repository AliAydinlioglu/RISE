namespace Rise.Client.Calendar;

public static class CalendarHelpers
{
    public static DateTime GetMondayOfWeek(DateTime date)
    {
        var daysFromMonday = (date.DayOfWeek - DayOfWeek.Monday + 7) % 7;
        return date.AddDays(-daysFromMonday);
    }
}