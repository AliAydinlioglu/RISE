using Ardalis.Result;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.Calendar.Tests;

public class GivenACalendarService
{
    [Fact]
    public async Task WhenUserRequestsCalendar_ThenCalendarShouldBeReturned()
    {
        var user = new UserDto("user123", "TIAO-1");
        var query = new FakeGetCalendarByClassGroup();
        var service = new CalendarService(query);
        
        var result = await service.GetCalendarAsync(user);
        
        result.IsSuccess.ShouldBeTrue();
        result.Value.ClassGroup.ShouldBe("TIAO-1");
    }
}

public class CalendarService
{
    public CalendarService(object fakeQuery)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(UserDto user)
    {
        throw new NotImplementedException();
    }
}

public static partial class CalendarResponse
{
    public class Get
    {
        public string ClassGroup { get; set; }
    }   
}