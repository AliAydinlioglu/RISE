using System.Security.Claims;
using FastEndpoints.Security;
using Rise.Services.Identity;
using Rise.Shared.Calendar;

namespace Rise.Server.Endpoints.Calendar;

public class GetCalendarEndpoint(ICalendarService service, ISessionContextProvider sessionProvider): EndpointWithoutRequest<Result<CalendarResponse.Get>>
{
    public override void Configure()
    {
        Get("/api/calendar");
        AllowAnonymous(); // TODO: aan te passen wanneer auth is geimplementeerd
    }

    public override async Task<Result<CalendarResponse.Get>> ExecuteAsync(CancellationToken ct)
    {
        var userId = sessionProvider.User?.ClaimValue(ClaimTypes.NameIdentifier);
    
        // TODO: aan te passen wanneer auth is geimplementeerd
        // if (string.IsNullOrEmpty(userId))
        //    return Result.Unauthorized("User not authenticated");

        return await service.GetCalendarAsync(userId);

    }
}