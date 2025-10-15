using Rise.Domain.Calendar;

namespace Rise.Domain.Tests.Calendar;

public class GivenADeadline
{
    [Fact]
    public void WhenDescriptionIsNull_ThenDeadlineShouldBeCreatedAnyways()
    {
        var deadline = new Deadline("Campus App", null, AValidTimestamp);

        deadline.TaskTitle.ShouldBe("Campus App");
        deadline.TaskDescription.ShouldBeNull();
        deadline.DeadlineTimestamp.ShouldBe(AValidTimestamp);
    }

    [Fact]
    public void WhenTaskTitleIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Deadline("", "Description", AValidTimestamp));
    }

    [Fact]
    public void WhenTaskTitleIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Deadline(null!, "Description", AValidTimestamp));
    }
    
    private static readonly DateTimeOffset
        AValidTimestamp = new(2025, 12, 14, 23, 59, 59, TimeSpan.Zero);
}