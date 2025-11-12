using Rise.Shared.Calendar;

namespace Rise.Client.Calendar;

public static class CalendarExtensions
{
    public static IEnumerable<CalendarViewItem> ToCalendarListItems(this CalendarResponse.Get response)
    {
        var items = response.Courses.SelectMany(course =>
            course.GenerateCourseSessions(response.AcademicSemester)
                .Concat(course.GenerateDeadlines())
                .Concat(course.GenerateExams())
        );

        return items.OrderBy(item => item.Date);
    }

    private static IEnumerable<CalendarViewItem> GenerateCourseSessions(
        this CalendarResponse.CourseInfo course,
        CalendarResponse.AcademicSemesterInfo semesterInfo
    )
    {
        var startDate = semesterInfo.StartDate.LocalDateTime;
        var endDate = semesterInfo.EndDate.LocalDateTime;

        return course.Lessons.SelectMany(lesson =>
            GenerateSessionsForLesson(lesson, course, startDate, endDate)
        );
    }

    private static IEnumerable<CalendarViewItem> GenerateSessionsForLesson(
        CalendarResponse.LessonInfo lesson,
        CalendarResponse.CourseInfo course,
        DateTime startDate,
        DateTime endDate
        )
    {
        var dayOfWeek = Enum.Parse<DayOfWeek>(lesson.Day, ignoreCase: true);
        var firstOccurrence = GetFirstDateForDay(startDate, dayOfWeek);

        for (var date = firstOccurrence; date <= endDate; date = date.AddDays(7))
        {
            yield return new CalendarViewItem
            {
                CourseId = course.CourseId,
                Type = CalendarViewItem.CalendarEventType.Course,
                Date = date,
                Title = course.CourseTitle,
                Description = course.Lecturer,
                Header = $"{lesson.StartTime} | {lesson.Room}"
            };
        }
    }

    private static IEnumerable<CalendarViewItem> GenerateDeadlines(this CalendarResponse.CourseInfo course)
    {
        return course.Deadlines.Select(it => new CalendarViewItem
        {
            CourseId = course.CourseId,
            Type = CalendarViewItem.CalendarEventType.Deadline,
            Date = it.DeadlineTimestamp.LocalDateTime,
            Title = it.TaskTitle,
            Description = it.TaskDescription,
            Header = course.CourseTitle
        });
    }

    private static IEnumerable<CalendarViewItem> GenerateExams(this CalendarResponse.CourseInfo course)
    {
        return course.Exams.Select(it =>
        {
            var examDateTime = it.ExamTimestamp.LocalDateTime;
            return new CalendarViewItem
            {
                CourseId = course.CourseId,
                Type = CalendarViewItem.CalendarEventType.Exam,
                Title = it.ExamTitle,
                Date = examDateTime,
                Header = $"{examDateTime:HH:mm} | {it.Room}"
            };
        });
    }

    private static DateTime GetFirstDateForDay(DateTime start, DayOfWeek targetDayOfWeek)
    {
        var daysOffset = ((int)targetDayOfWeek - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysOffset);
    }
}