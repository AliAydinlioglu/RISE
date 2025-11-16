using Rise.Shared.Courses;

namespace Rise.Client.Courses;

public static class CourseDetailsExtensions
{
    public static CourseDetailViewModels ToViewModel(this CourseDetailResponse.Get response) => new()
    {
        CourseTitle = response.CourseTitle,
        Lecturer = response.Lecturer,
        LessonDate = response.Lesson.Date,
        LessonTime = $"{response.Lesson.StartTime} - {response.Lesson.EndTime}",
        Location = new LocationViewModel
        {
            Campus = $"Campus {response.Campus.Name}",
            Adress =
                $"{response.Campus.Street} {response.Campus.HouseNumber}, {response.Campus.PostalCode} {response.Campus.City}",
            Room = response.Campus.Room
        },
        Announcements = response.Announcements.Select(it => new AnnouncementViewModel
        {
            Title = it.Title,
            Sender = it.Sender,
            Message = it.Message,
        }).ToList(),
        Deadlines = response.Deadlines.Select(it => new DeadlineViewModel
        {
            Date = it.DeadlineTimestamp.DateTime,
            Title = it.DeadlineTitle,
            Description = it.DeadlineDescription
        }).ToList()
    };
}