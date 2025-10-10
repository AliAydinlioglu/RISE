using Rise.Server.Endpoints.StudentActivities.Validators;
using Rise.Shared.StudentActivities;

namespace Rise.Server.Endpoints.StudentActivities;

public class Detail(IStudentActivitiesService studentActivitiesService) : Endpoint<StudentActivityRequest.Detail, Result<StudentActivityResponse.Detail>>
{
    public override void Configure()
    {
        Get("/api/student-activities/{Id}");
        AllowAnonymous();
    }
    
    public override Task<Result<StudentActivityResponse.Detail>> ExecuteAsync(StudentActivityRequest.Detail req, CancellationToken ctx)
    {
        
        return studentActivitiesService.GetDetailByIdAsync(req.Id, ctx);
    }
    
}