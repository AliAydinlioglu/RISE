using Ardalis.Result;
using Rise.Services.Calendar;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.Calendar.Tests;

public class GivenACalendarService
{
    private readonly CalendarService _service;

    public GivenACalendarService()
    {
        // Gemeenschappelijke setup
        var query = new FakeGetCalendarQuery();
        _service = new CalendarService(query);
    }
    
    [Fact]
    public async Task WhenUserRequestsCalendar_ThenCalendarShouldBeReturned()
    {
        var user = new UserDto("user123", "TIAO-1");
        
        var result = await _service.GetCalendarAsync(user);
        
        result.IsSuccess.ShouldBeTrue();
        var calendar = result.Value;

        calendar.ClassGroup.ShouldBe("TIAO-01");
        calendar.AcademicYear.ShouldBe("2024-2025");
        calendar.Period.Type.ShouldBe("SEM1");
        calendar.Courses.Count.ShouldBe(2);

        var riseCourse = calendar.Courses[0];
        riseCourse.CourseTitle.ShouldBe("RISE");
        riseCourse.Sessions.Count.ShouldBe(2);

        var fallCourse = calendar.Courses[1];
        fallCourse.CourseTitle.ShouldBe("FALL");
        fallCourse.Deadlines.Count.ShouldBe(1);
        fallCourse.Exams.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task WhenNonLoggedInUserRequestsCalendar_ThenCallShouldFail()
    {
        var result = await _service.GetCalendarAsync(null!);
        
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
        result.Errors.ShouldContain("User is null");
    }
}
