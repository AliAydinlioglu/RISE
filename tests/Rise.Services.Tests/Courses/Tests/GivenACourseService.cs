using Rise.Services.Courses;
using Rise.Services.Tests.Calendar;
using Rise.Shared.Courses;

namespace Rise.Services.Tests.Courses.Tests;

public class GivenACourseService
{
    private readonly ICourseService _service;
    private readonly FakeUserRepository _userRepository;

    public GivenACourseService()
    {
        var query = new FakeGetCourseDetailQuery();
        _userRepository = new FakeUserRepository();
        _service = new CourseService(query, _userRepository);
    }

    [Fact(DisplayName = "When user requests CourseDetail, then course detail should be returned succesfully")]
    public async Task HappyFlow()
    {
        var userId = "user123";
        _userRepository.AddUser(userId, "TIAO-01");
        
        var result = await _service.GetCourseDetailAsync(userId, 1, new DateOnly(2024, 11, 14));
        
        result.IsSuccess.ShouldBeTrue();
    }
}