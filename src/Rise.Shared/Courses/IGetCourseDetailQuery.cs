namespace Rise.Shared.Courses;

public interface IGetCourseDetailQuery
{
    Task<Result<CourseDetailResponse.Get>> ExecuteAsync(int courseId, DateOnly date, string userClassGroup);
}