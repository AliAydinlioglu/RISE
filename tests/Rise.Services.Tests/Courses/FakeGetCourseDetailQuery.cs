using Ardalis.Result;
using Rise.Shared.Courses;

namespace Rise.Services.Tests.Courses;

public class FakeGetCourseDetailQuery : IGetCourseDetailQuery
{
    public Task<Result<CourseDetailResponse.Get>> ExecuteAsync(int courseId, DateOnly date, string userClassGroup)
    {
        return Task.FromResult(Result.Success(new CourseDetailResponse.Get()));
    }
}