namespace Rise.Domain.Calendar;

public class Session : Entity
{
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public string Campus { get; private set; }
    public string Room { get; private set; }

    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    private Session() { }

    public Session(DayOfWeek dayOfWeek, TimeRange timeRange, string campus, string room)
    {
        DayOfWeek = dayOfWeek;
        TimeRange = Guard.Against.Null(timeRange);
        Campus = Guard.Against.NullOrWhiteSpace(campus);
        Room = Guard.Against.NullOrWhiteSpace(room);
    }
}