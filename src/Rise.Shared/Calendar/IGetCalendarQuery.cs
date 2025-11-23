namespace Rise.Shared.Calendar;

public interface IGetCalendarQuery
{
    Task<CalendarResponse.Get> ExecuteAsync(string userClassGroup);
}