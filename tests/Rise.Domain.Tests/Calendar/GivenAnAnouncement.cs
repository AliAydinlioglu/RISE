using Rise.Domain.Calendar;

namespace Rise.Domain.Tests.Calendar;

public class GivenAnAnouncement
{
    [Fact(DisplayName = "When Title is null, then exception is thrown")]
    public void WhenTitleIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() => new Announcement(
            null,
            AValidLecturer,
            "Message",
            ValidTimestamp));
    }

    [Fact(DisplayName = "When Title is empty, then exception is thrown")]
    public void WhenTitleIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() => new Announcement(
            "",
            AValidLecturer,
            "Message",
            ValidTimestamp));
    }

    [Fact(DisplayName = "When Sender is null, then exception is thrown")]
    public void WhenSenderIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() => new Announcement(
            "Title",
            null,
            "Message",
            ValidTimestamp));
    }

    [Fact(DisplayName = "When Message is null, then exception is thrown")]
    public void WhenMessageIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() => new Announcement(
            "Title",
            AValidLecturer,
            null,
            ValidTimestamp));
    }

    [Fact(DisplayName = "When Message is empty, then exception is thrown")]
    public void WhenMessageIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() => new Announcement(
            "Title",
            AValidLecturer,
            "",
            ValidTimestamp));
    }

    private static readonly DateTimeOffset ValidTimestamp = DateTimeOffset.UtcNow;
    private static readonly Lecturer AValidLecturer = new("Alice", "Bob");
}