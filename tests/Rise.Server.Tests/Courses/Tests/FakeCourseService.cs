using Ardalis.Result;
using Rise.Shared.Courses;

namespace Rise.Server.Tests.Courses.Tests;

public class FakeCourseService : ICourseService
{
    public Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date)
    {
        return Task.FromResult(Result.Success(new CourseDetailResponse.Get()));
    }
}