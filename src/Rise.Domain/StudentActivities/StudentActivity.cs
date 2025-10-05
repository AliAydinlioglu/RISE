using Rise.Domain.Exceptions;

namespace Rise.Domain.StudentActivities;

public class StudentActivity : Entity
{
    private readonly string _title = string.Empty;

    public string Title
    {
        get => _title;
        private init => _title = Guard.Against.NullOrWhiteSpace(value);
    }

    public string Description { get; private init; }

    private readonly DateTime _date;

    public DateTime Date
    {
        get => _date;
        private init => _date = Guard.Against.NullOrOutOfSQLDateRange(value);
    }

    private readonly DateTime _startTime;

    public DateTime StartTime
    {
        get => _startTime;
        private init => _startTime = Guard.Against.NullOrOutOfSQLDateRange(value);
    }

    private readonly DateTime _endTime;

    public DateTime EndTime
    {
        get => _endTime;
        private init => _endTime = Guard.Against.NullOrOutOfSQLDateRange(value);
    }

    public string ImageUrl { get; private init; }

    public Location Location { get; set; }
    public StudentClub StudentClub { get; set; }

    public StudentActivity(string title, string description, DateTime date, DateTime startTime, DateTime endTime,
        string imageUrl, Location location, StudentClub studentClub)
    {
        Title = title;
        Description = description;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        ValidateTimes();
        ImageUrl = imageUrl;
        Location = Guard.Against.Null(location);
        StudentClub = Guard.Against.Null(studentClub);
    }
    
    private void ValidateTimes()
    {
        if (_startTime >= _endTime)
            throw new InvalidTimeRangeException("Start time must be before end time.");
    }
}