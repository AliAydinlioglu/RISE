using Rise.Shared.Calendar;
using Rise.Shared.Identity;
using Rise.Shared.User;

namespace Rise.Services.Calendar;

public class CalendarService(IGetCalendarQuery query, IUserRepository userRepository) : ICalendarService
{
    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(string userId)
    {
        var classGroup = await userRepository.GetClassGroupAsync(userId);
        return await query.ExecuteAsync(classGroup!);
    }
}
