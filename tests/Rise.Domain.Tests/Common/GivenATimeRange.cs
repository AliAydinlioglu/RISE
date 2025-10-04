using Rise.Domain.Common;

namespace Rise.Domain.Tests.Common;

public class GivenATimeRange
{
    [Fact]
    public void WhenEndTimeBeforeStartTime_ShouldThrowException()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(8, 30);

        Should.Throw<ArgumentException>(() => new TimeRange(start, end));
    }

}