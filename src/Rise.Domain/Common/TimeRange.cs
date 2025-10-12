namespace Rise.Domain.Common;

public class TimeRange : ValueObject
{
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    private TimeRange() { }

    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be after StartTime", nameof(endTime));

        StartTime = startTime;
        EndTime = endTime;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}