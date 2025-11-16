using Microsoft.EntityFrameworkCore;
using Rise.Domain.Calendar;
using Rise.Domain.Common;
using Rise.Persistence.Queries.Calendar;
using Rise.TestDoubles.Fakers;

namespace Rise.Persistence.Tests.Calendar;

public class GivenAGetCalendarQuery: IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly GetCalendarQuery _query;
    private readonly FakeDateTimeService _dateTimeService;

    public GivenAGetCalendarQuery()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _dateTimeService = new FakeDateTimeService(new DateTime(2024, 11, 13));
        _query = new GetCalendarQuery(_context, _dateTimeService);
    }

    [Fact]
    public async Task WhenCalendarDataExists_ThenReturnsCompleteCalendar()
    {
        await SeedTestDataAsync();

        var result = await _query.ExecuteAsync("TIAO-01");

        result.IsSuccess.ShouldBeTrue();
        var calendar = result.Value;
        
        calendar.ClassGroup.ShouldBe("TIAO-01");
        calendar.AcademicYear.ShouldBe("2024-2025");
        calendar.AcademicSemester.Type.ShouldBe("SEM1");
        calendar.Courses.Count.ShouldBe(2);

        var riseCourse = calendar.Courses.First(c => c.CourseTitle == "RISE");
        riseCourse.Lecturer.ShouldBe("Alice Alisson");
        riseCourse.Lessons.Count.ShouldBe(2);

        var fallCourse = calendar.Courses.First(c => c.CourseTitle == "FALL");
        fallCourse.Deadlines.Count.ShouldBe(1);
        fallCourse.Exams.Count.ShouldBe(1);
    }
    
    [Fact]
    public async Task WhenNoCoursesForClassGroup_ThenReturnsEmptyCourseList()
    {
        await SeedTestDataAsync();

        var result = await _query.ExecuteAsync("TIAO-02");

        result.IsSuccess.ShouldBeTrue();
        result.Value.Courses.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task WhenNoAcademicSemesterExists_ThenExceptionIsThrown()
    {
        var academicSemester = new AcademicSemester(
            "2024-2025",
            SemesterType.Sem1,
            new DateRange(_dateTimeService.Now, _dateTimeService.Now.AddHours(1)),
            _dateTimeService.Now
        );
        
        _dateTimeService.SetDateTime(new DateTime(2025, 11, 13));
        var course = new Course("RISE", new Lecturer("Alice", "Alisson"), "TIAO-01", academicSemester);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        await Should.ThrowAsync<InvalidOperationException>(async () =>
            await _query.ExecuteAsync("TIAO-01"));
    }
    
    private async Task SeedTestDataAsync()
    {
        var academicSemester = new AcademicSemester(
            "2024-2025",
            SemesterType.Sem1,
            new DateRange(
                new DateTimeOffset(2024, 9, 22, 0, 0, 0, TimeSpan.Zero),
                new DateTimeOffset(2025, 1, 31, 0, 0, 0, TimeSpan.Zero)
            ),
            new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
        );
        _context.AcademicSemesters.Add(academicSemester);
        await _context.SaveChangesAsync();

        var riseCourse = new Course("RISE", new Lecturer("Alice", "Alisson"), "TIAO-01", academicSemester);
        var riseLesson1 = new Lesson(
            DayOfWeek.Monday,
            new TimeRange(new TimeOnly(8, 30), new TimeOnly(10, 30)),
            "Schoonmeersen",
            "GSCHB.2.001"
        );
        var riseLesson2 = new Lesson(
            DayOfWeek.Thursday,
            new TimeRange(new TimeOnly(10, 45), new TimeOnly(13, 0)),
            "Schoonmeersen",
            "GSCHB.2.001"
        );
        
        _context.Courses.Add(riseCourse);
        _context.Lessons.AddRange(riseLesson1, riseLesson2);

        var fallCourse = new Course("FALL", new Lecturer("Bob", "Bobson"), "TIAO-01", academicSemester);
        var deadline = new Deadline(
            "Campus App",
            "I like bananas",
            new DateTimeOffset(2025, 12, 14, 23, 59, 59, TimeSpan.Zero)
        );
        var exam = new Exam(
            "FALL - Theory",
            new DateTimeOffset(2025, 1, 10, 9, 0, 0, TimeSpan.Zero),
            "Schoonmeersen",
            "GSCHT.1.101"
        );

        _context.Courses.Add(fallCourse);
        _context.Deadlines.Add(deadline);
        _context.Exams.Add(exam);

        await _context.SaveChangesAsync();

        riseLesson1.GetType().GetProperty("CourseId")!.SetValue(riseLesson1, riseCourse.Id);
        riseLesson2.GetType().GetProperty("CourseId")!.SetValue(riseLesson2, riseCourse.Id);
        deadline.GetType().GetProperty("CourseId")!.SetValue(deadline, fallCourse.Id);
        exam.GetType().GetProperty("CourseId")!.SetValue(exam, fallCourse.Id);

        await _context.SaveChangesAsync();
    }

    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}