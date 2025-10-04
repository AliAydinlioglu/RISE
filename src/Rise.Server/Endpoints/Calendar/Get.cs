using Rise.Services.Identity;
using Rise.Shared.Calendar;
using Rise.Shared.Identity;

namespace Rise.Server.Endpoints.Calendar;

public class GetCalendarEndpoint(ICalendarService service, ISessionContextProvider sessionProvider): EndpointWithoutRequest<Result<CalendarResponse.Get>>
{
    public override void Configure() => Get("/api/calendar");

    public override async Task<Result<CalendarResponse.Get>> ExecuteAsync(CancellationToken ct)
    {
        var userId = sessionProvider.User?.GetUserId();
    
        if (string.IsNullOrEmpty(userId))
            return Result.Unauthorized("User not authenticated");

        var userDto = new UserDto(userId);
        return await service.GetCalendarAsync(userDto);

    }
}