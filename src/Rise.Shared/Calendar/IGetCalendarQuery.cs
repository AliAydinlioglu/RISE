using Rise.Shared.Identity;

namespace Rise.Shared.Calendar;

public interface IGetCalendarQuery
{
    Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup);
}