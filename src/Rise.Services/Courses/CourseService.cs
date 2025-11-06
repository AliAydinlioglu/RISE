using Rise.Shared.Courses;
using Rise.Shared.User;

namespace Rise.Services.Courses;

public class CourseService(IGetCourseDetailQuery query, IUserRepository userRepository) : ICourseService
{
    public async Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date)
    {
        var classGroup = await userRepository.GetClassGroupAsync(userId);
        return await query.ExecuteAsync(courseId, date, classGroup!);
    }
}