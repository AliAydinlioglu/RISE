using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Shared.Calendar;

namespace Rise.Persistence.Queries.Calendar;

public class GetCalendarQuery(ApplicationDbContext dbContext): IGetCalendarQuery
{
    public async Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup)
    {
        var now = DateTime.UtcNow;
        
        var academicSemester = await dbContext.AcademicSemesters
            .Where(it => it.DateRange.StartDate <= now && it.DateRange.EndDate >= now)
            .FirstOrDefaultAsync();
        
        if (academicSemester is null)
            throw new InvalidOperationException("No active semester");
        
        var courses = await dbContext.Courses
            .Where(it => it.ClassGroup == userClassGroup)
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
            Lecturer = course.Lecturer,
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