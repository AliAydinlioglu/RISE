namespace Rise.Domain.Calendar;

/// <summary>
/// Represents an announcement sent by a lecturer
/// </summary>
public class Announcement : Entity
{
    public string Title { get; private set; }
    public Lecturer Sender { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    
    public Course Course { get; private set; } = null!;
    
    private Announcement(){}

    public Announcement(string title, Lecturer sender, string message, DateTimeOffset timestamp)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        Sender = Guard.Against.Null(sender);
        Message = Guard.Against.NullOrWhiteSpace(message);
        Timestamp = Guard.Against.Null(timestamp);
    }
}