namespace Rise.Shared.Courses;

public static class CourseDetailResponse
{
    public class Get
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public LecturerInfo Lecturer { get; set; }
        public LessonDetail Lesson { get; set; }
        public CampusInfo Campus { get; set; }
        public List<AnnouncementInfo> Announcements { get; set; }
        public List<DeadlineInfo> Deadlines { get; set; }
    }

    public class LessonDetail
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class CampusInfo
    {
        public string Name { get; set; }
        public string Room { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
    }

    public class AnnouncementInfo
    {
        public string AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class DeadlineInfo
    {
        public string DeadlineId { get; set; }
        public string DeadlineTitle { get; set; }
        public string DeadlineDescription { get; set; }
        public DateTimeOffset DeadlineTimestamp { get; set; }
    }

    public class LecturerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}