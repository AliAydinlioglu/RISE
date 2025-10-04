using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Calendar;

public class GivenASession
{
    [Fact]
    public void WhenTimeRangeIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentNullException>(() =>
            new Session(DayOfWeek.Monday, null!, "Schoonmeersen", "GSCHB.2.001"));
    }
    
    [Fact]
    public void WhenCampusIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Session(DayOfWeek.Monday, AValidDateRange, "", "GSCHB.2.001"));
    }
    
    [Fact]
    public void WhenCampusIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Session(DayOfWeek.Monday, AValidDateRange, null!, "GSCHB.2.001"));
    }

    [Fact]
    public void WhenRoomIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Session(DayOfWeek.Monday, AValidDateRange, "Schoonmeersen", ""));
    }

    [Fact]
    public void WhenRoomIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Session(DayOfWeek.Monday, AValidDateRange, "Schoonmeersen", null!));
    }

    private static readonly TimeRange AValidDateRange = new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30));
}