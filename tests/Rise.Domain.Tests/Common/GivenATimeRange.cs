using Rise.Domain.Common;
using Rise.Domain.Exceptions;

namespace Rise.Domain.Tests.Common;

public class GivenATimeRange
{
    [Fact]
    public void WhenEndTimeBeforeStartTime_ShouldThrowException()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(8, 30);

        Should.Throw<InvalidTimeRangeException>(() => new TimeRange(start, end));
    }

}