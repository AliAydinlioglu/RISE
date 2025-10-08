using Rise.Domain.Exceptions;

namespace Rise.Domain.StudentActivities;

public class StudentActivity : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string ImageUrl { get; private set; }
    public Location Location { get; set; }
    public StudentClub StudentClub { get; set; }

    public StudentActivity(){}
    public StudentActivity(string title, string description, DateTime date, DateTime startTime, DateTime endTime,
        string imageUrl, Location location, StudentClub studentClub)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.NullOrOutOfSQLDateRange(date);
        Guard.Against.NullOrOutOfSQLDateRange(startTime);
        Guard.Against.NullOrOutOfSQLDateRange(endTime);
        Guard.Against.Null(location);
        Guard.Against.Null(studentClub);

        Title = title;
        Description = description;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        ValidateTimes();
        ImageUrl = imageUrl;
        Location = location;
        StudentClub = studentClub;

    }

    private void ValidateTimes()
    {
        if (StartTime >= EndTime)
            throw new InvalidTimeRangeException("Start time must be before end time.");
    }
}