namespace Rise.Client.Calendar;

public class CalendarViewItem
{
    public required CalendarEventType Type { get; set; }
    public required DateTime Date { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Header { get; set; }

    public enum CalendarEventType
    {
        Course,
        Deadline,
        Exam
    }
}