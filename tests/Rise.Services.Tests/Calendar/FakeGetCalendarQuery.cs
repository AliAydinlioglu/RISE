using Ardalis.Result;
using Rise.Shared.Calendar;

namespace Rise.Services.Tests.Calendar;

public class FakeGetCalendarQuery: IGetCalendarQuery
{
    public Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup)
    {
        var calendar = new CalendarResponse.Get
        {
            ClassGroup = userClassGroup
        };
        
        return Task.FromResult(Result.Success(calendar));
    }
}