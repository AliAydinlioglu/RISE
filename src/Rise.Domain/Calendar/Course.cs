namespace Rise.Domain.Calendar;

public class Course : Entity
{
    public string Title { get; private set; }
    public string Lecturer { get; private set; }
    public string ClassGroup { get; private set; }
    
    private readonly List<Session> _sessions = new();
    public IReadOnlyCollection<Session> Sessions => _sessions.AsReadOnly();

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