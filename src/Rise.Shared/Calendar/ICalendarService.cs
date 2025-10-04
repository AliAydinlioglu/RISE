using Rise.Shared.Identity;

namespace Rise.Shared.Calendar;

public interface ICalendarService
{
    Task<Result<CalendarResponse.Get>> GetCalendarAsync(UserDto? user);
}