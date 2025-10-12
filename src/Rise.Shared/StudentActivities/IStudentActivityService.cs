using Rise.Shared.Common;

namespace Rise.Shared.StudentActivities;

public interface IStudentActivityService
{
    Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx);
    
    Task<Result<StudentActivityResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx);
}