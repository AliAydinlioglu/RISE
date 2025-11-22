using Rise.Domain.Calendar;
using Rise.Shared.Courses;

namespace Rise.Persistence.Queries.Courses.Mappers;

public static class CourseDetailMapper
{
    public static CourseDetailResponse.Get MapToResponse(
        Course course,
        DateOnly date,
        DateTimeOffset dateAsOffset)
    {
        var lesson = course.Lessons.First(lesson => lesson.DayOfWeek == date.DayOfWeek);

        return new CourseDetailResponse.Get
        {
            CourseId = course.Id.ToString(),
            CourseTitle = course.Title,
            Lecturer = course.Lecturer.ToLecturerInfo(),
            Lesson = lesson.ToLessonDetail(date),
            Campus = lesson.ToCampusInfo(),
            Announcements = course.ToAnnouncementsInfo(),
            Deadlines = course.ToUpcommingDeadlinesInfo(dateAsOffset)
        };
    }

    private static CourseDetailResponse.LessonDetail ToLessonDetail(this Lesson lesson, DateOnly date)
    {
        return new CourseDetailResponse.LessonDetail
        {
            Date = date.ToString(),
            StartTime = lesson.TimeRange.StartTime.ToString("HH:mm"),
            EndTime = lesson.TimeRange.EndTime.ToString("HH:mm")
        };
    }

    private static CourseDetailResponse.CampusInfo ToCampusInfo(this Lesson lesson)
    {
        // Hardcoded for now to Campus Schoonmeersen
        return new CourseDetailResponse.CampusInfo
        {
            Name = lesson.Campus,
            Room = lesson.Room,
            Street = "Valentin Vaerwyck",
            HouseNumber = 1,
            PostalCode = 9000,
            City = "Gent"
        };
    }

    private static List<CourseDetailResponse.AnnouncementInfo> ToAnnouncementsInfo(this Course course)
    {
        return course.Announcements
            .OrderByDescending(it => it.Timestamp)
            .Select(it => new CourseDetailResponse.AnnouncementInfo
            {
                AnnouncementId = it.Id.ToString(),
                Title = it.Title,
                Sender = $"{it.Sender.FirstName} {it.Sender.LastName}",
                Message = it.Message,
                Timestamp = it.Timestamp
            })
            .ToList();
    }

    private static List<CourseDetailResponse.DeadlineInfo> ToUpcommingDeadlinesInfo(this Course course,
        DateTimeOffset fromDate)
    {
        return course.Deadlines
            .Where(it => it.DeadlineTimestamp >= fromDate)
            .OrderBy(it => it.DeadlineTimestamp)
            .Select(it => new CourseDetailResponse.DeadlineInfo
            {
                DeadlineId = it.Id.ToString(),
                DeadlineTitle = it.TaskTitle,
                DeadlineDescription = it.TaskDescription ?? string.Empty,
                DeadlineTimestamp = it.DeadlineTimestamp
            })
            .ToList();
    }

    private static CourseDetailResponse.LecturerInfo ToLecturerInfo(this Lecturer courseLecturer)
    {
        return new CourseDetailResponse.LecturerInfo
        {
            FirstName = courseLecturer.FirstName,
            LastName = courseLecturer.LastName,
        };
    }
}