using Rise.Domain.Common;
using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class ContactPeriodShould
{
    private readonly DateOnly _date = new(2025, 11, 6);
    private readonly List<TimeRange> _timeRanges = new()
    {
        new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)),
        new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0))
    };
    
    [Fact]
    public void BeCreated_WithValidParameters()
    {
        var period = new ContactPeriod(_date, _timeRanges);

        period.ShouldNotBeNull();
        period.ContactDate.ShouldBe(_date);
        period.ContactHours.ShouldBe(_timeRanges);
        period.ContactHours.Count.ShouldBe(2);
    }

    [Fact]
    public void BeCreated_WithSingleTimeRange()
    {
        var singleTimeRange = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
        };

        var period = new ContactPeriod(_date, singleTimeRange);

        period.ShouldNotBeNull();
        period.ContactHours.Count.ShouldBe(1);
        period.ContactHours[0].StartTime.ShouldBe(new TimeOnly(9, 0));
        period.ContactHours[0].EndTime.ShouldBe(new TimeOnly(17, 0));
    }

    [Fact]
    public void BeEqual_WhenDateAndTimeRangesAreTheSame()
    {
        var period1 = new ContactPeriod(_date, _timeRanges);
        var period2 = new ContactPeriod(_date, _timeRanges);

        period1.ShouldBe(period2);
    }

    [Fact]
    public void NotBeEqual_WhenDatesAreDifferent()
    {
        var date1 = new DateOnly(2025, 11, 6);
        var date2 = new DateOnly(2025, 11, 7);
        var period1 = new ContactPeriod(date1, _timeRanges);
        var period2 = new ContactPeriod(date2, _timeRanges);

        period1.ShouldNotBe(period2);
    }

    [Fact]
    public void NotBeEqual_WhenTimeRangesAreDifferent()
    {
        var timeRanges1 = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
        };
        var timeRanges2 = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(10, 0), new TimeOnly(18, 0))
        };
        var period1 = new ContactPeriod(_date, timeRanges1);
        var period2 = new ContactPeriod(_date, timeRanges2);

        period1.ShouldNotBe(period2);
    }

    [Fact]
    public void BeEqual_WhenTimeRangesAreInDifferentOrder()
    {
        var timeRanges1 = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)),
            new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0))
        };
        var timeRanges2 = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0)),
            new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0))
        };
        var period1 = new ContactPeriod(_date, timeRanges1);
        var period2 = new ContactPeriod(_date, timeRanges2);

        period1.ShouldBe(period2);
    }

    [Fact]
    public void HandleEmptyTimeRanges()
    {
        var emptyTimeRanges = new List<TimeRange>();

        var period = new ContactPeriod(_date, emptyTimeRanges);

        period.ShouldNotBeNull();
        period.ContactHours.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(2025, 11, 6)]
    [InlineData(2025, 12, 25)]
    [InlineData(2026, 1, 1)]
    public void BeCreated_WithDifferentDates(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);
        var timeRanges = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
        };

        var period = new ContactPeriod(date, timeRanges);

        period.ShouldNotBeNull();
        period.ContactDate.ShouldBe(date);
    }

    [Fact]
    public void BeCreated_WithMultipleNonOverlappingTimeRanges()
    {
        var date = new DateOnly(2025, 11, 6);
        var timeRanges = new List<TimeRange>
        {
            new TimeRange(new TimeOnly(8, 0), new TimeOnly(10, 0)),
            new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0)),
            new TimeRange(new TimeOnly(14, 0), new TimeOnly(16, 0))
        };

        var period = new ContactPeriod(date, timeRanges);

        period.ShouldNotBeNull();
        period.ContactHours.Count.ShouldBe(3);
    }
}

