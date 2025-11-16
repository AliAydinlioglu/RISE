using Ardalis.Result;
using Rise.Shared.Courses;

namespace Rise.Client.Courses;

public class FakeCourseService(
    bool withDelay, 
    bool hasAnnouncements, 
    bool hasDeadlines) : ICourseService
{
    public async Task<Result<CourseDetailResponse.Get>> GetCourseDetailAsync(string userId, int courseId, DateOnly date)
    {
        if (withDelay)
            await Task.Delay(100);

        return await Task.FromResult(Result.Success(_response));
    }

    private readonly CourseDetailResponse.Get _response = new()
    {
        CourseTitle = "Test Course",
        Lecturer = new CourseDetailResponse.LecturerInfo
        {
            FirstName = "Dr.",
            LastName = "Test"
        },
        Lesson = new CourseDetailResponse.LessonDetail
        {
            Date = "13 november 2024",
            StartTime = "09:00",
            EndTime = "12:00"
        },
        Campus = new CourseDetailResponse.CampusInfo
        {
            Name = "Schoonmeersen",
            Street = "Valentin Vaerwyck",
            HouseNumber = 1,
            PostalCode = 9000,
            City = "Gent",
            Room = "B.2.040"
        },
        Announcements = GenerateAnnounements(hasAnnouncements),
        Deadlines = GenerateDeadlines(hasDeadlines)
    };

    private static List<CourseDetailResponse.AnnouncementInfo> GenerateAnnounements(bool hasAnnouncements)
    {
        return hasAnnouncements
            ? new List<CourseDetailResponse.AnnouncementInfo>
            {
                new()
                {
                    Title = "Test Announcement",
                    Sender = "Test sender",
                    Message = "Test message",
                }
            }
            : [];
    }

    private static List<CourseDetailResponse.DeadlineInfo> GenerateDeadlines(bool hasDeadlines)
    {
        return hasDeadlines
            ? new List<CourseDetailResponse.DeadlineInfo>
            {
                new()
                {
                    DeadlineId = "1",
                    DeadlineTitle = "Test Deadline",
                    DeadlineDescription = "Test Deadline description",
                    DeadlineTimestamp = DateTime.Now
                }
            }
            : [];
    }
}