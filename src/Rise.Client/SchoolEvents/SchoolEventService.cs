using System.Net.Http.Json;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;

namespace Rise.Client.SchoolEvents;

public class SchoolEventService(HttpClient httpClient) : ISchoolEventService
{
    public async Task<Result<SchoolEventResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<SchoolEventResponse.Index>>($"/api/school-events?skip={request.Skip}&take={request.Take}", cancellationToken: ctx);
        return result!;
    }

    public async Task<Result<SchoolEventResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<SchoolEventResponse.Detail>>($"/api/school-events/{id}", cancellationToken: ctx);
        return result!;
    }
}