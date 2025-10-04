using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Common;

public class GivenADateRange
{
    [Fact]
    public void WhenEndDateBeforeStartDate_ThenThrowException()
    {
        var start = new DateTimeOffset(2025, 12, 14, 0, 0, 0, TimeSpan.Zero);
        var end = new DateTimeOffset(2025, 9, 22, 0, 0, 0, TimeSpan.Zero);

        Should.Throw<ArgumentException>(() => new DateRange(start, end));
 
    }
}