namespace Rise.Client.Courses;

public class CourseDetailViewModels
{
    public string CourseTitle { get; set; }
    public string Lecturer { get; set; }
    public string LessonDate { get; set; }
    public string LessonTime { get; set; }
    public LocationViewModel Location { get; set; }
    public IList<AnnouncementViewModel> Announcements { get; set; }
    public IList<DeadlineViewModel> Deadlines { get; set; }
}

public class LocationViewModel
{
    public string Campus { get; set; }
    public string Adress { get; set; }
    public string Room { get; set; }
}

public class AnnouncementViewModel
{
    public string Title { get; set; }
    public string Sender { get; set; }
    public string Message { get; set; }
}

public class DeadlineViewModel
{
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}