using Ardalis.Result;
using Rise.Services.Calendar;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.Calendar.Tests;

public class GivenACalendarService
{
    [Fact]
    public async Task WhenUserRequestsCalendar_ThenCalendarShouldBeReturned()
    {
        var user = new UserDto("user123", "TIAO-1");
        var query = new FakeGetCalendarQuery();
        var service = new CalendarService(query);
        
        var result = await service.GetCalendarAsync(user);
        
        result.IsSuccess.ShouldBeTrue();
        result.Value.ClassGroup.ShouldBe("TIAO-1");
    }
    
    [Fact]
    public async Task WhenNonLoggedInUserRequestsCalendar_ThenCallShouldFail()
    {
        var query = new FakeGetCalendarQuery();
        var service = new CalendarService(query);
        
        var result = await service.GetCalendarAsync(null!);
        
        result.IsSuccess.ShouldBeFalse();
    }
}
