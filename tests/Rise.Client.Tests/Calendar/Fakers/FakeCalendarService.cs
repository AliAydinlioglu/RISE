using Ardalis.Result;
using Rise.Shared.Calendar;
using Rise.TestDoubles;

namespace Rise.Client.Calendar.Fakers;

public class FakeCalendarService(bool withDelay, bool withDeadlines = true) : ICalendarService
{
    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(string userId)
    {
        if (withDelay)
            await Task.Delay(100);
        
        var response = CalendarObjectMother.BuildGetResponse(withDeadlines);

        return await Task.FromResult(Result.Success(response));
    }
}