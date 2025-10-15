using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Calendar;

public class GivenALesson
{
    [Fact]
    public void WhenTimeRangeIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentNullException>(() =>
            new Lesson(DayOfWeek.Monday, null!, "Schoonmeersen", "GSCHB.2.001"));
    }
    
    [Fact]
    public void WhenCampusIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Lesson(DayOfWeek.Monday, AValidDateRange, "", "GSCHB.2.001"));
    }
    
    [Fact]
    public void WhenCampusIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Lesson(DayOfWeek.Monday, AValidDateRange, null!, "GSCHB.2.001"));
    }

    [Fact]
    public void WhenRoomIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Lesson(DayOfWeek.Monday, AValidDateRange, "Schoonmeersen", ""));
    }

    [Fact]
    public void WhenRoomIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Lesson(DayOfWeek.Monday, AValidDateRange, "Schoonmeersen", null!));
    }

    private static readonly TimeRange AValidDateRange = new(new TimeOnly(8, 30), new TimeOnly(10, 30));
}