using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Calendar;

public class GivenACourse
{
    [Fact]
    public void WhenTitleIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("", "Alice", "TIAO-01", AValidAcademicSemester));
    }
    [Fact]
    public void WhenTitleIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course(null!, "Alice", "TIAO-01", AValidAcademicSemester));
    }

    [Fact]
    public void WhenLecturerIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "", "TIAO-01", AValidAcademicSemester));
    }

    [Fact]
    public void WhenLecturerIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", null!, "TIAO-01", AValidAcademicSemester));
    }

    [Fact]
    public void WhenClassGroupIsEmpty_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "Alice", "", AValidAcademicSemester));
    }

    [Fact]
    public void WhenClassGroupIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "Alice", null!, AValidAcademicSemester));
    }

    [Fact]
    public void WhenAcademicSemesterIsNull_ThenExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Course("RISE", "Alice","TIAO-01", null!));
    }

    [Fact]
    public void WhenAddingALesson_ThenALessonShouldBeSuccesfullyAddedToTheCourse()
    {
        var course = new Course("Irrelevant", "Irrelevant", "Irrelevant", AValidAcademicSemester);
        course.Lessons.Count.ShouldBe(0);

        course.AddLesson(AValidLesson);
        course.Lessons.Count.ShouldBe(1);
    }

    [Fact]
    public void WhenAddingADeadline_ThenADeadlineShouldBeSuccesfullyAddedToTheCourse()
    {
        var course = new Course("Irrelevant", "Irrelevant", "Irrelevant", AValidAcademicSemester);
        course.Deadlines.Count.ShouldBe(0);

        course.AddDeadline(AValidDeadline);
        course.Deadlines.Count.ShouldBe(1);
    }

    [Fact]
    public void WhenAddingAExam_ThenAExamShouldBeSuccesfullyAddedToTheCourse()
    {
        var course = new Course("Irrelevant", "Irrelevant", "Irrelevant", AValidAcademicSemester);
        course.Exams.Count.ShouldBe(0);

        course.AddExam(AValidExam);
        course.Exams.Count.ShouldBe(1);
    }

    public static readonly AcademicSemester AValidAcademicSemester
        = new("2025-2026", SemesterType.Sem1, new DateRange(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1)), DateTimeOffset.Now);

    private static readonly Lesson AValidLesson 
        = new(DayOfWeek.Monday, new TimeRange(TimeOnly.MinValue, TimeOnly.MaxValue), "Schoonmeersen", "GSCHB.2.001");

    public static readonly Deadline AValidDeadline
        = new("Deadline RISE", "CampusApp Demo", DateTimeOffset.Now);

    public static readonly Exam AValidExam
        = new("FALL - Theory", DateTimeOffset.Now, "Schoonmeersen", "GSCHT.1.101");
}