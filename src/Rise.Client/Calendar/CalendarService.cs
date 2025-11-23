using System.Net.Http.Json;
using Rise.Shared.Calendar;

namespace Rise.Client.Calendar;

public class CalendarService(HttpClient httpClient) : ICalendarService
{
    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(string userId)
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<Result<CalendarResponse.Get>>("/api/calendar");
            return result!;
        }
        catch (Exception _)
        {
            return Result<CalendarResponse.Get>.Error();
        }
    }
}