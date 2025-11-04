namespace Rise.Domain.Contact;

/// <summary>
/// This is a ssupporting class for information about 
/// contact days, openingshours, etc.
/// </summary>
/// <param name="openingHours"></param>
public class ContactPeriod(IDictionary<DateOnly, List<TimeRange>> openingHours): ValueObject
{
    public IDictionary<DateOnly, List<TimeRange>> OpeningHours { get; private set; } = openingHours;

    /// <summary>
    /// Check if it is open on a specific date time
    /// </summary>
    /// <param name="date"></param>
    /// <param name="hour"></param>
    /// <returns>bool</returns>
    public bool IsOpenOn(DateOnly date, TimeOnly hour)
    {
        if (OpeningHours.TryGetValue(date, out var hours))
        {
            return hours.Any(x => hour.IsBetween(x.StartTime, x.EndTime));
        }

        return false;
    }
    /// <summary>
    /// Check if it is open at this current date - time
    /// </summary>
    /// <returns>bool</returns>
    public bool IsOpen()
    {
        DateTime currentDate = DateTime.Now;
        return IsOpenOn(
            DateOnly.FromDateTime(currentDate),
            TimeOnly.FromDateTime(currentDate));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var kvp in OpeningHours.OrderBy(kvp => kvp.Key))
        {
            yield return kvp.Key;

            // Sorteer de TimeRanges ook voor voorspelbare volgorde
            foreach (var timeRange in kvp.Value.OrderBy(tr => tr.StartTime).ThenBy(tr => tr.EndTime))
            {
                yield return timeRange;
            }
        }
    }
}