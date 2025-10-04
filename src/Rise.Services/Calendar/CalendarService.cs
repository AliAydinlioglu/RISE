using Rise.Shared.Calendar;
using Rise.Shared.Identity;

namespace Rise.Services.Calendar;

public class CalendarService: ICalendarService
{
    private readonly IGetCalendarQuery _getCalendarQuery;

    public CalendarService(IGetCalendarQuery query)
    {
        _getCalendarQuery = query;
    }

    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(UserDto user)
    {
        return await _getCalendarQuery.ExecuteAsync(user.ClassGroup);
    }
}
