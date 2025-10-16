using Rise.Shared.Calendar;
using Shouldly;

namespace Rise.Client.Calendar;

public class GivenCalendarExtensions
{
    private readonly List<CalendarViewItem> _items;

    public GivenCalendarExtensions()
    {
        _items = CreateTestCalendarResponse().ToCalendarListItems().ToList();
    }

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
        //todo RI2526T2-53 exam.Header.ShouldBe("09:00 | GSCHC.1.404");
    }

    private List<CalendarViewItem> GetItemsOfType(CalendarViewItem.CalendarEventType type) =>
        _items.Where(it => it.Type == type).ToList();

    private CalendarResponse.Get CreateTestCalendarResponse()
    {
        return new CalendarResponse.Get
        {
            ClassGroup = "TIAO",
            AcademicYear = "2024-2025",
            AcademicSemester = new CalendarResponse.AcademicSemesterInfo
            {
                AcademicSemesterId = "1",
                Type = "Sem1",
                StartDate = new DateTimeOffset(2024, 9, 22, 0, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero),
                ExamStartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            Courses =
            [
                new CalendarResponse.CourseInfo
                {
                    CourseId = "1",
                    CourseTitle = "RISE",
                    Lecturer = "B. Alice",
                    Lessons =
                    [
                        new CalendarResponse.LessonInfo
                        {
                            Day = "MONDAY",
                            StartTime = "08:30",
                            EndTime = "10:30",
                            Campus = "Schoonmeersen",
                            Room = "GSCHB.2.001"
                        },
                        new CalendarResponse.LessonInfo
                        {
                            Day = "WEDNESDAY",
                            StartTime = "12:30",
                            EndTime = "16:00",
                            Campus = "Schoonmeersen",
                            Room = "GSCHB.2.001"
                        }
                    ],
                    Deadlines =
                    [
                        new CalendarResponse.DeadlineInfo
                        {
                            DeadlineId = "1",
                            TaskTitle = "Deadline RISE",
                            TaskDescription = "CampusApp Demo",
                            DeadlineTimestamp = new DateTimeOffset(2024, 11, 12, 23, 59, 59, TimeSpan.Zero)
                        }
                    ],
                    Exams = []
                },
                new CalendarResponse.CourseInfo
                {
                    CourseId = "2",
                    CourseTitle = "Modern Data Structures",
                    Lecturer = "A. Bob",
                    Lessons =
                    [
                        new CalendarResponse.LessonInfo
                        {
                            Day = "THURSDAY",
                            StartTime = "12:30",
                            EndTime = "14:30",
                            Campus = "Schoonmeersen",
                            Room = "GSCHC.1.404"
                        }
                    ],
                    Deadlines = [],
                    Exams = [
                        new CalendarResponse.ExamInfo
                        {
                            ExamId = "1",
                            ExamTitle = "Examen MDS",
                            ExamTimestamp = new DateTimeOffset(2025, 1, 12, 8, 0, 0, TimeSpan.Zero),
                            Campus = "Schoonmeersen",
                            Room = "GSCHC.1.404"
                        }
                    ]
                }
            ]
        };
    }
}