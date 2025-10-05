using Rise.Domain.Calendar;

namespace Rise.Domain.Tests.Calendar;

public class GivenACourse
{
    [Fact]
    public void WhenTitleIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("", "Alice", "TIAO-01"));
    }
    [Fact]
    public void WhenTitleIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course(null!, "Alice", "TIAO-01"));
    }

    [Fact]
    public void WhenLecturerIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "", "TIAO-01"));
    }

    [Fact]
    public void WhenLecturerIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", null!, "TIAO-01"));
    }

    [Fact]
    public void WhenClassGroupIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "Alice", ""));
    }

    [Fact]
    public void WhenClassGroupIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "Alice", null!));
    }
}