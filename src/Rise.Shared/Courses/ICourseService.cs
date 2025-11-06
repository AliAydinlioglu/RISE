namespace Rise.Shared.Courses;

public interface ICourseService
{
    Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date);
}