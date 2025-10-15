namespace Rise.Domain.Calendar;

/// <summary>
/// Represents a due date for a specific academic task or assignment within a course
/// </summary>
public class Deadline : Entity
{
    public string TaskTitle { get; private set; }
    public string? TaskDescription { get; private set; }
    public DateTimeOffset DeadlineTimestamp { get; private set; }

    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    private Deadline() { }

    public Deadline(string taskTitle, string? taskDescription, DateTimeOffset deadlineTimestamp)
    {
        TaskTitle = Guard.Against.NullOrWhiteSpace(taskTitle);
        TaskDescription = taskDescription;
        DeadlineTimestamp = deadlineTimestamp;
    }
}
