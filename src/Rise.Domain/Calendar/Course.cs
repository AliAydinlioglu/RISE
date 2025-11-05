namespace Rise.Domain.Calendar;

/// <summary>
/// Represents an academic course taught within a specific class group and led by a lecturer.
/// </summary>
public class Course : Entity
{
    public string Title { get; private set; }
    public string Lecturer { get; private set; }
    public string ClassGroup { get; private set; }
    
    public int AcademicSemesterId { get; private set; }
    public AcademicSemester AcademicSemester { get; private set; } = null!;
    
    private readonly List<Lesson> _lessons = new();
    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    private readonly List<Deadline> _deadlines = new();
    public IReadOnlyCollection<Deadline> Deadlines => _deadlines.AsReadOnly();

    private readonly List<Exam> _exams = new();
    public IReadOnlyCollection<Exam> Exams => _exams.AsReadOnly();

    private readonly List<Announcement> _announcements = new();
    public IReadOnlyCollection<Announcement> Announcements => _announcements.AsReadOnly();

    private Course() { }

    public Course(string title, string lecturer, string classGroup, AcademicSemester academicSemester)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        Lecturer = Guard.Against.NullOrWhiteSpace(lecturer);
        ClassGroup = Guard.Against.NullOrWhiteSpace(classGroup);
        AcademicSemester = Guard.Against.Null(academicSemester);
    }
    
    public void AddLesson(Lesson lesson)
    {
        _lessons.Add(lesson);
    }
    
    public void AddDeadline(Deadline deadline)
    {
        _deadlines.Add(deadline);
    }
    
    public void AddExam(Exam exam)
    {
        _exams.Add(exam);
    }

}