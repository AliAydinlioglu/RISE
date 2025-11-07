using System.Security.Claims;
using FastEndpoints.Security;
using Rise.Services.Identity;
using Rise.Shared.Courses;

namespace Rise.Server.Endpoints.Courses;

public class Detail(ICourseService service, ISessionContextProvider sessionProvider): Endpoint<CourseDetailRequest, Result<CourseDetailResponse.Get>>
{
    public override void Configure()
    {
        Get("/api/lessen/{Id}");
        AllowAnonymous(); // TODO: aan te passen wanneer auth is geimplementeerd
    }

    public override async Task<Result<CourseDetailResponse.Get>> ExecuteAsync(CourseDetailRequest req, CancellationToken ct)
    {
        var userId = sessionProvider.User?.ClaimValue(ClaimTypes.NameIdentifier);
        
        // TODO: aan te passen wanneer auth is geimplementeerd
        // if (string.IsNullOrEmpty(userId))
        //    return Result.Unauthorized("User not authenticated");
        
        return await service.GetCourseDetailAsync(userId, req.Id, req.Datum);
    }
}