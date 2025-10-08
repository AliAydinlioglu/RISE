using Rise.Shared.Common;

namespace Rise.Shared.StudentActivities;

public interface IStudentActivitiesService
{
    Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx);
}