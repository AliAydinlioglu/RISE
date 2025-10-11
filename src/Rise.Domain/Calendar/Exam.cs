namespace Rise.Domain.Calendar;

/// <summary>
/// Represents a scheduled exam for a specific course within the academic calendar.
/// </summary>
public class Exam : Entity
{
    public string Title { get; private set; }
    public DateTimeOffset ExamTimestamp { get; private set; }
    public string Campus { get; private set; }
    public string Room { get; private set; }

    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    private Exam() { }

    public Exam(string title, DateTimeOffset examTimestamp, string campus, string room)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        ExamTimestamp = examTimestamp;
        Campus = Guard.Against.NullOrWhiteSpace(campus);
        Room = Guard.Against.NullOrWhiteSpace(room);
    }
}
