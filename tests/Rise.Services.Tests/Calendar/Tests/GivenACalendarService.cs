using Ardalis.Result;
using Rise.Services.Calendar;
using Rise.Shared.Calendar;

namespace Rise.Services.Tests.Calendar.Tests;

public class GivenACalendarService
{
    private readonly CalendarService _service;
    private readonly FakeUserRepository _userRepository;

    public GivenACalendarService()
    {
        var query = new FakeGetCalendarQuery();
        _userRepository = new FakeUserRepository();
        _service = new CalendarService(query, _userRepository);
    }
    
    [Fact]
    public async Task WhenUserRequestsCalendar_ThenCalendarShouldBeReturned()
    {
        var userId = "user123";
        _userRepository.AddUser(userId, "TIAO-01");
        
        var result = await _service.GetCalendarAsync(userId);
        
        result.IsSuccess.ShouldBeTrue();
        AssertCalendarContent(result);
    }

    private static void AssertCalendarContent(Result<CalendarResponse.Get> result)
    {
        var calendar = result.Value;

        calendar.ClassGroup.ShouldBe("TIAO-01");
        calendar.AcademicYear.ShouldBe("2024-2025");
        calendar.AcademicSemester.Type.ShouldBe("SEM1");
        calendar.Courses.Count.ShouldBe(2);

        var riseCourse = calendar.Courses[0];
        riseCourse.CourseTitle.ShouldBe("RISE");
        riseCourse.Sessions.Count.ShouldBe(2);

        var fallCourse = calendar.Courses[1];
        fallCourse.CourseTitle.ShouldBe("FALL");
        fallCourse.Deadlines.Count.ShouldBe(1);
        fallCourse.Exams.Count.ShouldBe(2);
    }
}