using Rise.Domain.Exceptions;

namespace Rise.Domain.StudentActivities;

public class StudentActivity : Entity
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public string? ImageUrl { get; private set; }
    public Location Location { get; set; }
    public StudentClub StudentClub { get; set; }

    private StudentActivity(){}
    public StudentActivity(string title, string description, DateTimeOffset date, TimeRange timeRange,
        string imageUrl, Location location, StudentClub studentClub)
    {
        Title = Guard.Against.NullOrWhiteSpace(title);
        Description = description;
        Date = Guard.Against.NullOrOutOfSQLDateRange(date.UtcDateTime);
        TimeRange = Guard.Against.Null(timeRange);
        ImageUrl = imageUrl;
        Location = Guard.Against.Null(location);
        StudentClub = Guard.Against.Null(studentClub);
    }
}