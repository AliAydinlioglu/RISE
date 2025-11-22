using System.Net.Http.Json;
using Rise.Shared.Courses;

namespace Rise.Client.Courses;

public class CourseService(HttpClient httpClient): ICourseService
{
    public async Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date)
    {
        var result = await httpClient.GetFromJsonAsync<Result<CourseDetailResponse.Get>>($"/api/lessen/{courseId}?datum={date}");
        return result!;
    }
}