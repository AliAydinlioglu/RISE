using Rise.Shared.Calendar;
using Rise.Shared.Identity;

namespace Rise.Server.Endpoints.Calendar;

public class GetCalendarEndpoint(ICalendarService service): EndpointWithoutRequest<Result<CalendarResponse.Get>>
{
    public override void Configure()
    {
        Get("/api/calendar");
    }
    
    public async Task<Result<CalendarResponse.Get>> ExecuteAsync()
    {
        var user = new UserDto("123", "TIAO-01");
        return await service.GetCalendarAsync(user);
    }
}