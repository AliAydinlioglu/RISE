using Rise.Shared.Calendar;
using Rise.Shared.Identity;

namespace Rise.Services.Calendar;

public class CalendarService(IGetCalendarQuery query) : ICalendarService
{
    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(UserDto? user)
    {
        if (user is null)
            return Result<CalendarResponse.Get>.Unauthorized("User is null");

        return await query.ExecuteAsync(user.ClassGroup);
    }
}
