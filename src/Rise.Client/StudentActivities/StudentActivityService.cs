using Rise.Shared.Common;
using Rise.Shared.StudentActivities;
using System.Net.Http.Json;

namespace Rise.Client.StudentActivities;

public class StudentActivityService(HttpClient httpClient) : IStudentActivityService
{
    public async Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<StudentActivityResponse.Index>>($"/api/student-activities?skip={request.Skip}&take={request.Take}", cancellationToken: ctx);
        return result!;
    }

    public async Task<Result<StudentActivityResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<StudentActivityResponse.Detail>>($"/api/student-activities/{id}", cancellationToken: ctx);
        return result!;
    }
}