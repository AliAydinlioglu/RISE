using Rise.TestDoubles;
using Shouldly;

namespace Rise.Client.Calendar;

public class GivenCalendarExtensions
{
    private readonly List<CalendarViewItem> _items = CalendarObjectMother.BuildGetResponse().ToCalendarListItems().ToList();

    [Fact]
    public void WhenConvertingResponseToViewItems_ThenItemsShouldBeMappedCorrectly()
    {
        _items.ShouldNotBeEmpty();

        var first = _items.First();
        first.ShouldBeOfType<CalendarViewItem>();
        first.Type.ShouldBeOfType<CalendarViewItem.CalendarEventType>();
        first.Title.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void WhenGeneratingRecurringCourseInformation_ThenItShouldGenerateCourseInformationCorrectly()
    {
        var courseSessions = _items
            .Where(it => it.Type == CalendarViewItem.CalendarEventType.Course)
            .ToList();

        courseSessions.Count.ShouldBeGreaterThan(10);
    }

    [Fact]
    public void WhenConvertingResponseToViewItems_ShouldIncludeDeadlines()
    {
        var deadlines = GetItemsOfType(CalendarViewItem.CalendarEventType.Deadline);
        deadlines.ShouldNotBeEmpty();
    }

    [Fact]
    public void WhenConvertingResponseToViewItems_ShouldIncludeExams()
    {
        var exams = GetItemsOfType(CalendarViewItem.CalendarEventType.Exam);
        exams.ShouldNotBeEmpty();
    }

    [Fact]
    public void WhenSettingAHeaderForCourseSession_ThenHeaderShouldContainTimeAndRoom()
    {
        var riseMonday = _items.First(it =>
            it is { Type: CalendarViewItem.CalendarEventType.Course, Title: "RISE", Date.DayOfWeek: DayOfWeek.Monday }
        );

        riseMonday.Header.ShouldBe("08:30 | GSCHB.2.001");
    }

    [Fact]
    public void WhenSettingAHeaderForADeadline_ThenHeaderShouldBeCourseTitle()
    {
        var deadline = GetItemsOfType(CalendarViewItem.CalendarEventType.Deadline).First();
        deadline.Header.ShouldBe("RISE");
    }

    [Fact]
    public void WhenSettingAHeaderForAnExam_ThenHeaderShouldContainTimeAndRoom()
    {
        var exam = GetItemsOfType(CalendarViewItem.CalendarEventType.Exam).First();
        exam.Header.ShouldBe("09:00 | GSCHC.1.404");
    }

    private List<CalendarViewItem> GetItemsOfType(CalendarViewItem.CalendarEventType type) =>
        _items.Where(it => it.Type == type).ToList();
}