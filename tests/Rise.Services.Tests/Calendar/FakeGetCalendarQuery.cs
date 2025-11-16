using Ardalis.Result;
using Rise.Shared.Calendar;
using Rise.TestDoubles;

namespace Rise.Services.Tests.Calendar;

public class FakeGetCalendarQuery: IGetCalendarQuery
{
    public Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup)
    {
        return Task.FromResult(Result.Success(CalendarObjectMother.BuildGetResponse()));
    }
}