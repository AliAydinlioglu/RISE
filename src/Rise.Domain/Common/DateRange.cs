namespace Rise.Domain.Common;

public class DateRange : ValueObject
{
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset EndDate { get; }
    
    private DateRange() {  }
    
    public DateRange(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date", nameof(endDate));
        
        StartDate = startDate;
        EndDate = endDate;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}