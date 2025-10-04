using Ardalis.Result;
using Rise.Shared.Calendar;
using Rise.Shared.Identity;

namespace Rise.Server.Tests.Calendar.Tests;

public class GivenACalendarEndpoint
{
    [Fact]
    public async Task HandleAsync_ShouldReturnCalendar()
    {
        var service = new FakeCalendarService();
        var endpoint = new GetCalendarEndpoint(service);

        var result = await endpoint.HandleAsync();

        result.IsSuccess.ShouldBeTrue();
    }
}

public class GetCalendarEndpoint
{
    public GetCalendarEndpoint(FakeCalendarService service)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<object>> HandleAsync()
    {
        throw new NotImplementedException();
    }
}

public class FakeCalendarService: ICalendarService
{
    public Task<Result<CalendarResponse.Get>> GetCalendarAsync(UserDto? user)
    {
        throw new NotImplementedException();
    }
}