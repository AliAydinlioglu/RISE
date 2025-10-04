using Rise.Server.Endpoints.Calendar;
using Rise.Shared.Calendar;

namespace Rise.Server.Tests.Calendar.Tests;

public class GivenACalendarEndpoint
{
    [Fact]
    public async Task HandleAsync_ShouldReturnCalendar()
    {
        ICalendarService service = new FakeCalendarService();
        var endpoint = new GetCalendarEndpoint(service);

        var result = await endpoint.ExecuteAsync();

        result.IsSuccess.ShouldBeTrue();
    }
}
