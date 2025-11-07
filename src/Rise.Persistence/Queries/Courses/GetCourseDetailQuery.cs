using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Domain.Common;
using Rise.Persistence.Queries.Courses.Mappers;
using Rise.Shared.Courses;
using Serilog;

namespace Rise.Persistence.Queries.Courses;

public class GetCourseDetailQuery(ApplicationDbContext dbContext) : IGetCourseDetailQuery
{
    public async Task<Result<CourseDetailResponse.Get>> ExecuteAsync(
        int courseId,
        DateOnly date,
        string userClassGroup)
    {
        var course = await FetchCourse(courseId);
        if (course is null)
        {
            Log.Warning($"Course with Id {courseId} not found.");
            return Result.NotFound("Course not found");
        }

        var validationResult = ValidateCourse(course, date, userClassGroup);
        if (!validationResult.IsSuccess)
            return validationResult;

        var dateAsOffset = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue));
        var response = CourseDetailMapper.MapToResponse(course, date, dateAsOffset);
        
        return Result.Success(response);
    }

    private async Task<Course?> FetchCourse(int courseId)
    {
        return await dbContext.Courses
            .Include(c => c.AcademicSemester)
                .ThenInclude(s => s.DateRange)
            .Include(c => c.Lessons)
                .ThenInclude(l => l.TimeRange)
            .Include(c => c.Announcements)
                .ThenInclude(a => a.Sender)
            .Include(c => c.Deadlines)
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    private static Result<CourseDetailResponse.Get> ValidateCourse(
        Course course,
        DateOnly date,
        string userClassGroup)
    {
        var userIsEnrolledInCourse = course.ClassGroup == userClassGroup;
        if (!userIsEnrolledInCourse)
        {
            Log.Warning($"Course with {course.Id} is not assigned to classgroup: {userClassGroup}.");
            return Result.Forbidden();
        }

        var dateAsOffset = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue));
        if (!IsDateWithinSemester(dateAsOffset, course.AcademicSemester.DateRange))
        {
            Log.Warning($"Date {dateAsOffset} is outside the academic semester {course.AcademicSemester.Id}.");
            return Result.Invalid(new ValidationError("Date is outside the academic semester"));
        }

        var lesson = course.Lessons.FirstOrDefault(l => l.DayOfWeek == date.DayOfWeek);
        if (lesson is null)
        {
            Log.Warning($"No lesson scheduled on {date.DayOfWeek} for this course");
            return Result.NotFound($"No lesson scheduled on {date.DayOfWeek} for this course");
        }

        return Result.Success();
    }

    private static bool IsDateWithinSemester(DateTimeOffset date, DateRange semesterDateRange)
    {
        return date >= semesterDateRange.StartDate && date <= semesterDateRange.EndDate;
    }
}