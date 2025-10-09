namespace Rise.Domain.Calendar;

/// <summary>
/// Represents an academic course taught within a specific class group and led by a lecturer.
/// </summary>
public class Course : Entity
{
    public string Title { get; private set; }
    public string Lecturer { get; private set; }
    public string ClassGroup { get; private set; }
    
    private readonly List<Lesson> _lessons = new();
    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    private readonly List<Deadline> _deadlines = new();
    public IReadOnlyCollection<Deadline> Deadlines => _deadlines.AsReadOnly();

    private readonly List<Exam> _exams = new();
    public IReadOnlyCollection<Exam> Exams => _exams.AsReadOnly();

    private Course() { }

    public Course(string title, string lecturer, string classGroup)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        Lecturer = Guard.Against.NullOrWhiteSpace(lecturer);
        ClassGroup = Guard.Against.NullOrWhiteSpace(classGroup);
    }

}