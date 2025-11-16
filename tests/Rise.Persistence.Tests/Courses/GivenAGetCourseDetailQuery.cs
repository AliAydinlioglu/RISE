using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Domain.Common;
using Rise.Persistence.Queries.Courses;

namespace Rise.Persistence.Tests.Courses;

public class GivenAGetCourseDetailQuery : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly GetCourseDetailQuery _query;
    private AcademicSemester _academicSemester = null!;
    private Course _riseCourse = null!;

    public GivenAGetCourseDetailQuery()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _query = new GetCourseDetailQuery(_context);
    }

    [Fact(DisplayName = "When CourseDetail exists, then returns complete CourseDetail")]
    public async Task HappyFlow()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 11, 4); 

        var result = await _query.ExecuteAsync(_riseCourse.Id, date, "TIAO-01");

        result.IsSuccess.ShouldBeTrue();
        var detail = result.Value;
        
        detail.CourseId.ShouldBe(_riseCourse.Id.ToString());
        detail.CourseTitle.ShouldBe("RISE");
        detail.Lecturer.ShouldBe("Alice Johnson");
        detail.Lesson.Date.ShouldBe("4/11/2024");
        detail.Lesson.StartTime.ShouldBe("08:30");
        detail.Lesson.EndTime.ShouldBe("10:30");
        detail.Campus.Name.ShouldBe("Schoonmeersen");
        detail.Campus.Room.ShouldBe("GSCHB.2.001");
        detail.Campus.Street.ShouldBe("Valentin Vaerwyck");
        detail.Campus.HouseNumber.ShouldBe(1);
        detail.Campus.PostalCode.ShouldBe(9000);
        detail.Campus.City.ShouldBe("Gent");
    }

    [Fact(DisplayName = "When multiple announcements exist, then returns them sorted by date")]
    public async Task AnnouncementsAreSortedByDate()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 11, 4); // Monday

        var result = await _query.ExecuteAsync(_riseCourse.Id, date, "TIAO-01");

        result.IsSuccess.ShouldBeTrue();
        var detail = result.Value;
        
        detail.Announcements.Count.ShouldBe(2);
        detail.Announcements[0].Title.ShouldBe("Recent Announcement");
        detail.Announcements[0].Sender.ShouldBe("Alice Johnson");
        detail.Announcements[1].Title.ShouldBe("Old Announcement");
        
        detail.Deadlines.Count.ShouldBe(1);
        detail.Deadlines[0].DeadlineTitle.ShouldBe("Project Deadline");
    }

    [Fact(DisplayName = "When course is not found, then returns NotFound")]
    public async Task WhenCourseNotFound_ThenReturnsNotFound()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 11, 4);

        var result = await _query.ExecuteAsync(9999, date, "TIAO-01");

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(Ardalis.Result.ResultStatus.NotFound);
        result.Errors.First().ShouldBe("Course not found");
    }

    [Fact(DisplayName = "When user is not enrolled in course, then returns Forbidden")]
    public async Task WhenUserNotEnrolledInCourse_ThenReturnsForbidden()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 11, 4);

        var result = await _query.ExecuteAsync(_riseCourse.Id, date, "TIAO-02");

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(Ardalis.Result.ResultStatus.Forbidden);
    }

    [Fact(DisplayName = "When date outside semester, then returns Invalid")]
    public async Task WhenDateOutsideSemester_ThenReturnsInvalid()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 8, 15);

        var result = await _query.ExecuteAsync(_riseCourse.Id, date, "TIAO-01");

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(Ardalis.Result.ResultStatus.Invalid);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Date is outside the academic semester");
    }

    [Fact(DisplayName = "When no lesson on given day, then returns NotFound")]
    public async Task WhenNoLessonOnGivenDay_ThenReturnsNotFound()
    {
        await SeedTestDataAsync();
        var date = new DateOnly(2024, 11, 5);

        var result = await _query.ExecuteAsync(_riseCourse.Id, date, "TIAO-01");

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(Ardalis.Result.ResultStatus.NotFound);
        result.Errors.First().ShouldContain("No lesson scheduled on Tuesday");
    }

    [Fact(DisplayName = "When multiple lessons on different days, then returns correct lesson")]
    public async Task WhenMultipleLessonsOnDifferentDays_ThenReturnsCorrectLesson()
    {
        await SeedTestDataAsync();
        var thursdayDate = new DateOnly(2024, 11, 7);

        var result = await _query.ExecuteAsync(_riseCourse.Id, thursdayDate, "TIAO-01");

        result.IsSuccess.ShouldBeTrue();
        var detail = result.Value;
        
        detail.Lesson.StartTime.ShouldBe("10:45");
        detail.Lesson.EndTime.ShouldBe("13:00");
    }
    
    private async Task SeedTestDataAsync()
    {
        _academicSemester = new AcademicSemester(
            "2024-2025",
            SemesterType.Sem1,
            new DateRange(
                new DateTimeOffset(2024, 9, 22, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            ),
            new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
        );
        _context.AcademicSemesters.Add(_academicSemester);
        await _context.SaveChangesAsync();

        _riseCourse = new Course("RISE", new Lecturer("Alice", "Johnson"), "TIAO-01", _academicSemester);
        
        var mondayLesson = new Lesson(
            DayOfWeek.Monday,
            new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)),
            "Schoonmeersen",
            "GSCHB.2.001"
        );
        _riseCourse.AddLesson(mondayLesson);
        
        var thursdayLesson = new Lesson(
            DayOfWeek.Thursday,
            new TimeRange(new TimeOnly(10, 45), new TimeOnly(13, 0)),
            "Schoonmeersen",
            "GSCHB.2.001"
        );
        _riseCourse.AddLesson(thursdayLesson);

        var recentAnnouncement = new Announcement(
            "Recent Announcement",
            new Lecturer("Alice", "Johnson"),
            "This is a recent message",
            new DateTimeOffset(2024, 11, 3, 14, 0, 0, TimeSpan.Zero)
        );
        _riseCourse.AddAnnouncement(recentAnnouncement);
        
        var oldAnnouncement = new Announcement(
            "Old Announcement",
            new Lecturer("Alice", "Johnson"),
            "This is an older message",
            new DateTimeOffset(2024, 11, 1, 10, 0, 0, TimeSpan.Zero)
        );
        _riseCourse.AddAnnouncement(oldAnnouncement);

        var deadline = new Deadline(
            "Project Deadline",
            "Complete the project",
            new DateTimeOffset(2024, 12, 14, 23, 59, 59, TimeSpan.Zero)
        );
        _riseCourse.AddDeadline(deadline);

        _context.Courses.Add(_riseCourse);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}