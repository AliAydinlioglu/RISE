using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Shared;
using Rise.Shared.Calendar;

namespace Rise.Persistence.Queries.Calendar;

public class GetCalendarQuery(ApplicationDbContext dbContext, IDateTimeService dateTimeService): IGetCalendarQuery
{
    public async Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup)
    {
        var now = dateTimeService.Now;
        
        var academicSemester = dbContext.AcademicSemesters
            .Include(academicSemester => academicSemester.DateRange)
            .AsEnumerable()
            .FirstOrDefault(it => it.DateRange.StartDate <= now && it.DateRange.EndDate >= now);
        
        if (academicSemester is null)
            throw new InvalidOperationException("No active semester");
        
        var courses = await dbContext.Courses
            .Where(it => it.ClassGroup == userClassGroup && it.AcademicSemester == academicSemester)
            .Include(it => it.Lessons)
            .Include(it => it.Deadlines)
            .Include(it => it.Exams)
            .ToListAsync();

        var response = new CalendarResponse.Get
        {
            ClassGroup = userClassGroup,
            AcademicYear = academicSemester.AcademicYear,
            AcademicSemester = MapAcademicSemester(academicSemester),
            Courses = courses.Select(it => MapCourse(it)).ToList()
        };
        
        return Result.Success(response);
    }
    
    private static CalendarResponse.AcademicSemesterInfo MapAcademicSemester(AcademicSemester academicSemester)
    {
        return new CalendarResponse.AcademicSemesterInfo
        {
            AcademicSemesterId = academicSemester.Id.ToString(),
            Type = academicSemester.Type.ToString().ToUpperInvariant(),
            StartDate = academicSemester.DateRange.StartDate,
            EndDate = academicSemester.DateRange.EndDate,
            ExamStartDate = academicSemester.ExamStartDate
        };
    }

    private static CalendarResponse.CourseInfo MapCourse(Course course)
    {
        return new CalendarResponse.CourseInfo
        {
            CourseId = course.Id.ToString(),
            CourseTitle = course.Title,
            Lecturer = $"{course.Lecturer.FirstName} {course.Lecturer.LastName}", // TODO convert this to an object
            Lessons = course.Lessons.Select(it => MapLesson(it)).ToList(),
            Deadlines = course.Deadlines.Select(it => MapDeadline(it)).ToList(),
            Exams = course.Exams.Select(it => MapExam(it)).ToList()
        };
    }
    
    private static CalendarResponse.LessonInfo MapLesson(Lesson lesson)
    {
        return new CalendarResponse.LessonInfo
        {
            Day = lesson.DayOfWeek.ToString(),
            StartTime = lesson.TimeRange.StartTime.ToString("HH:mm"),
            EndTime = lesson.TimeRange.EndTime.ToString("HH:mm"),
            Campus = lesson.Campus,
            Room = lesson.Room
        };
    }
    
    private static CalendarResponse.DeadlineInfo MapDeadline(Deadline deadline)
    {
        return new CalendarResponse.DeadlineInfo
        {
            DeadlineId = deadline.Id.ToString(),
            TaskTitle = deadline.TaskTitle,
            TaskDescription = deadline.TaskDescription ?? string.Empty,
            DeadlineTimestamp = deadline.DeadlineTimestamp
        };
    }

    private static CalendarResponse.ExamInfo MapExam(Exam exam)
    {
        return new CalendarResponse.ExamInfo
        {
            ExamId = exam.Id.ToString(),
            ExamTitle = exam.Title,
            ExamTimestamp = exam.ExamTimestamp,
            Campus = exam.Campus,
            Room = exam.Room
        };
    }

}