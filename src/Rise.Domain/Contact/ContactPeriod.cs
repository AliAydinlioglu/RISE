namespace Rise.Domain.Contact;

/// <summary>
/// This is a supporting class for information about 
/// contact days, openingshours, etc.
/// </summary>
/// <param name="openingHours"></param>
public class ContactPeriod : ValueObject
{
    public DateOnly ContactDate { get; private set; }
    public List<TimeRange> ContactHours { get; private set; }

    private ContactPeriod() { }

    public ContactPeriod(DateOnly date, List<TimeRange> timeRanges)
    {
        ContactDate = date;
        ContactHours = timeRanges;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ContactDate;
        foreach (var tr in ContactHours.OrderBy(t => t.StartTime).ThenBy(t => t.EndTime))
        {
            yield return tr;
        }
    }
}