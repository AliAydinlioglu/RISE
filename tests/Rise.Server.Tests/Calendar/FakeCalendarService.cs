using Ardalis.Result;
using Rise.Shared.Calendar;
using Rise.TestDoubles;

namespace Rise.Server.Tests.Calendar;

public class FakeCalendarService : ICalendarService
{
    public Task<Result<CalendarResponse.Get>> GetCalendarAsync(string userId)
    {
        return Task.FromResult(Result.Success(CalendarObjectMother.BuildGetResponse()));
    }
}