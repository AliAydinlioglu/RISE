using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Server.Endpoints.StudentActivities;

public class Index(IStudentActivityService studentActivityService) : Endpoint<QueryRequest.SkipTake, Result<StudentActivityResponse.Index>>
{
    public override void Configure()
    {
        Get("/api/student-activities");
        AllowAnonymous(); 
    }

    public override Task<Result<StudentActivityResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return studentActivityService.GetIndexAsync(req, ct);
    }
}