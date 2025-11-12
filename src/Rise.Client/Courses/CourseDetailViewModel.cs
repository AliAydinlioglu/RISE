namespace Rise.Client.Courses;

public class CourseDetailViewModel
{
    public string CourseTitle { get; set; }
    public string Lecturer { get; set; }
    public string LessonDate { get; set; }
    public string LessonTime { get; set; }
    public LocationViewModel Location { get; set; }
}

public class LocationViewModel
{
    public string Campus { get; set; }
    public string Adress { get; set; }
    public string Room { get; set; }
}