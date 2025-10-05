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
        
        var period = await dbContext.Periods
            .Where(it => it.DateRange.StartDate <= now && it.DateRange.EndDate >= now)
            .FirstOrDefaultAsync();
        
        var courses = await dbContext.Courses
            .Where(it => it.ClassGroup == userClassGroup)
            .Include(it => it.Sessions)
            .Include(it => it.Deadlines)
            .Include(it => it.Exams)
            .ToListAsync();

        var response = new CalendarResponse.Get
        {
            ClassGroup = userClassGroup,
            AcademicYear = period!.AcademicYear,
            Period = MapPeriod(period),
            Courses = courses.Select(it => MapCourse(it)).ToList()
        };
        
        return Result.Success(response);
    }
    
    private static CalendarResponse.PeriodInfo MapPeriod(Period period)
    {
        return new CalendarResponse.PeriodInfo
        {
            PeriodId = period.Id.ToString(),
            Type = period.Type.ToString().ToUpperInvariant(),
            StartDate = period.DateRange.StartDate,
            EndDate = period.DateRange.EndDate,
            ExamStartDate = period.ExamStartDate
        };
    }

    private static CalendarResponse.CourseInfo MapCourse(Course course)
    {
        return new CalendarResponse.CourseInfo
        {
            CourseId = course.Id.ToString(),
            CourseTitle = course.Title,
            Lecturer = course.Lecturer,
            Sessions = course.Sessions.Select(it => MapSession(it)).ToList(),
            Deadlines = course.Deadlines.Select(it => MapDeadline(it)).ToList(),
            Exams = course.Exams.Select(it => MapExam(it)).ToList()
        };
    }
    
    private static CalendarResponse.SessionInfo MapSession(Session session)
    {
        return new CalendarResponse.SessionInfo
        {
            Day = session.DayOfWeek.ToString(),
            StartTime = session.TimeRange.StartTime.ToString("HH:mm"),
            EndTime = session.TimeRange.EndTime.ToString("HH:mm"),
            Campus = session.Campus,
            Room = session.Room
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