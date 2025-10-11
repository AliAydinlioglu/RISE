using Rise.Domain.Calendar;

namespace Rise.Domain.Tests.Calendar;

public class GivenAnExam
{
    [Fact]
    public void WhenTitleIsEmpty_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam("", AValidTimestamp, "Schoonmeersen", "GSCHT.1.101"));
    }
    
    [Fact]
    public void WhenTitleIsNull_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam(null!, AValidTimestamp, "Schoonmeersen", "GSCHT.1.101"));
    }
    
    [Fact]
    public void WhenCampusIsEmpty_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam("RISE - Theory", AValidTimestamp, "", "GSCHT.1.101"));
    }
    
    [Fact]
    public void WhenCampusIsNull_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam("RISE - Theory", AValidTimestamp, null!, "GSCHT.1.101"));
    }

    [Fact]
    public void WhenRoomIsEmpty_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam("RISE - Theory", AValidTimestamp, "Schoonmeersen", ""));
    }

    [Fact]
    public void WhenRoomIsNull_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Exam("RISE - Theory", AValidTimestamp, "Schoonmeersen", null!));
    }
    
    private static readonly DateTimeOffset AValidTimestamp = new(2025, 12, 14, 23, 59, 59, TimeSpan.Zero);
}