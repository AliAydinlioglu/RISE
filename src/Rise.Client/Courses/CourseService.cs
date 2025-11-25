using System.Net.Http.Json;
using Rise.Shared.Courses;

namespace Rise.Client.Courses;

public class CourseService(HttpClient httpClient): ICourseService
{
    public async Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/lessen/{courseId}?datum={date:yyyy-MM-dd}");
            var result = await response.Content.ReadFromJsonAsync<Result<CourseDetailResponse.Get>>();
            
            return result ?? Result.NotFound("Geen response van server");
        }
        catch (Exception ex)
        {
            return Result.Error($"Onverwachte fout: {ex.Message}");
        }
    }
}