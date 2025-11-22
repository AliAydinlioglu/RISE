using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Persistence.SeedData;

public static class CalendarSeeder
{
    private static ApplicationDbContext _dbContext = null!;
    private static Random _random = null!;
    private static List<Lecturer> _lecturers = null!;

    private static readonly List<string> Rooms =
    [
        "GSCHB.1.001", "GSCHC.1.404", "GSCHG.0.201", "IAT-POT.418", "GSCHB.3.503"
    ];
    
    private static readonly List<(string FirstName, string LastName)> LecturerData =
    [
        ("Robert", "C. Martin"),
        ("Eric", "Evans"),
        ("Martin", "Fowler"),
        ("Kent", "Beck"),
        ("Vaughn", "Vernon"),
        ("Alberto", "Brandolini")
    ];

    private static Lecturer GetRandomLecturer()
    {
        return _lecturers[_random.Next(_lecturers.Count)];
    }

    public static async Task Seed(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _random = new Random();

        _lecturers = CreateLecturerSeedData();
        await dbContext.Lecturers.AddRangeAsync(_lecturers);
        await dbContext.SaveChangesAsync();

        var academicSemesters = CreateAcademicSemesterSeedData();
        await dbContext.AcademicSemesters.AddRangeAsync(academicSemesters);
        await dbContext.SaveChangesAsync();

        var courses = CreateCourseSeedData(academicSemesters);
        await dbContext.Courses.AddRangeAsync(courses);
        await dbContext.SaveChangesAsync(); // Courses moeten EERST geseed worden zodat ze een ID hebben (foreign key)

        await dbContext.Lessons.AddRangeAsync(CreateLessonSeedData(courses));
        await dbContext.Deadlines.AddRangeAsync(CreateDeadlineSeedData(courses));
        await dbContext.Exams.AddRangeAsync(CreateExamSeedData(courses));
        await dbContext.Announcements.AddRangeAsync(CreateAnnouncementSeedData(courses));

        await dbContext.SaveChangesAsync();
    }

    private static List<Lecturer> CreateLecturerSeedData()
    {
        if (_dbContext.Lecturers.Any())
            return _dbContext.Lecturers.ToList();

        return LecturerData
            .Select(data => new Lecturer(data.FirstName, data.LastName))
            .ToList();
    }

    private static List<AcademicSemester> CreateAcademicSemesterSeedData()
    {
        if (_dbContext.AcademicSemesters.Any())
            return [];

        return
        [
            new AcademicSemester("2025-2026", SemesterType.Sem1,
                new DateRange(
                    new DateTimeOffset(2025, 9, 22, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(2026, 1, 31, 0, 0, 0, TimeSpan.Zero)
                ),
                new DateTimeOffset(2026, 1, 31, 0, 0, 0, TimeSpan.Zero)
            ),

            new AcademicSemester("2025-2026", SemesterType.Sem2,
                new DateRange(
                    new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero)
                ),
                new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero)
            )
        ];
    }

    private static List<Course> CreateCourseSeedData(List<AcademicSemester> semesters)
    {
        if (_dbContext.Courses.Any())
            return [];

        var sem1 = semesters.First(s => s.Type == SemesterType.Sem1);
        var sem2 = semesters.First(s => s.Type == SemesterType.Sem2);

        var courses = new List<Course>
        {
            new("OOSDI", GetRandomLecturer(), "TIAO1", sem1),
            new("OOSDII", GetRandomLecturer(), "TIAO1", sem2),
            new("Software Analyse", GetRandomLecturer(), "TIAO1", sem1),
            new("IT Fundamentals", GetRandomLecturer(), "TIAO1", sem1),
            new("ASDI", GetRandomLecturer(), "TIAO2", sem2),
            new("ASDII", GetRandomLecturer(), "TIAO2", sem2),
            new("Functionele Analyse", GetRandomLecturer(), "TIAO2", sem1),
            new("IT Professional", GetRandomLecturer(), "TIAO2", sem1),
            new("Webservices", GetRandomLecturer(), "TIAO2", sem2),
            new("RISE", GetRandomLecturer(), "TIAO3", sem1),
            new("C#", GetRandomLecturer(), "TIAO3", sem2),
            new("Modern Data Architectures", GetRandomLecturer(), "TIAO3", sem2),
        };

        return courses;
    }

    private static List<Lesson> CreateLessonSeedData(List<Course> courses)
    {
        if (_dbContext.Lessons.Any())
            return [];

        var lessons = new List<Lesson>();

        var timeRanges = new List<TimeRange>
        {
            new(new TimeOnly(8, 30), new TimeOnly(10, 30)),
            new(new TimeOnly(10, 30), new TimeOnly(12, 30)),
            new(new TimeOnly(8, 30), new TimeOnly(12, 30)),
            new(new TimeOnly(13, 30), new TimeOnly(15, 30)),
            new(new TimeOnly(15, 30), new TimeOnly(17, 30)),
            new(new TimeOnly(13, 30), new TimeOnly(17, 30)),
        };

        for (var i = 0; i < 30; i++)
        {
            var day = (DayOfWeek)_random.Next((int)DayOfWeek.Monday, (int)DayOfWeek.Friday + 1);
            var timeRange = timeRanges[_random.Next(timeRanges.Count)];
            var room = Rooms[_random.Next(Rooms.Count)];
            var course = courses[_random.Next(courses.Count)];

            var lessonTimeRange = new TimeRange(timeRange.StartTime, timeRange.EndTime); // Refresh reference (necessary for EF)
            var lesson = new Lesson(day, lessonTimeRange, "Schoonmeersen", room);
            course.AddLesson(lesson);

            lessons.Add(lesson);
        }

        return lessons;
    }

    private static List<Deadline> CreateDeadlineSeedData(List<Course> courses)
    {
        if (_dbContext.Deadlines.Any())
            return [];

        var deadlines = new List<Deadline>();

        foreach (var course in courses)
        {
            var count = _random.Next(0, 2);
            var semester = course.AcademicSemester;
            var totalDaysInSemester = (semester.ExamStartDate - semester.DateRange.StartDate).Days;
            var randomOffset = _random.Next(0, totalDaysInSemester);

            for (var i = 0; i < count; i++)
            {
                var deadline = new Deadline(
                    taskTitle: $"{course.Title} - Taak {i + 1}",
                    taskDescription: "Inleveren via Chamilo",
                    deadlineTimestamp: semester.DateRange.StartDate.AddDays(randomOffset)
                );

                course.AddDeadline(deadline);
                deadlines.Add(deadline);
            }
        }

        return deadlines;
    }

    private static List<Exam> CreateExamSeedData(List<Course> courses)
    {
        if (_dbContext.Exams.Any())
            return [];

        var exams = new List<Exam>();

        foreach (var course in courses)
        {
            var count = _random.Next(0, 3);
            var semester = course.AcademicSemester;
            var totalDaysInExamPeriod = (semester.DateRange.EndDate - semester.ExamStartDate).Days;
            var randomOffset = _random.Next(0, totalDaysInExamPeriod);

            for (var i = 0; i < count; i++)
            {
                var exam = new Exam(
                    title: course.Title,
                    examTimestamp: semester.ExamStartDate.AddDays(randomOffset),
                    campus: "Schoonmeersen",
                    room: Rooms[_random.Next(Rooms.Count)]
                );
                course.AddExam(exam);
                exams.Add(exam);
            }
        }

        return exams;
    }

    private static List<Announcement> CreateAnnouncementSeedData(List<Course> courses)
    {
        if (_dbContext.Announcements.Any())
            return [];

        var announcements = new List<Announcement>();

        foreach (var course in courses)
        {
            var count = _random.Next(0, 3);
            var semester = course.AcademicSemester;
            var totalDaysInSemester = (semester.ExamStartDate - semester.DateRange.StartDate).Days;
            var randomOffset = _random.Next(0, totalDaysInSemester);

            for (var i = 0; i < count; i++)
            {
                var announcement = new Announcement(
                    title: $"Vaknieuwstitel {i + 1}",
                    sender: GetRandomLecturer(),
                    message: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    timestamp: semester.DateRange.StartDate.AddDays(randomOffset)
                );
                course.AddAnnouncement(announcement);
                announcements.Add(announcement);
            }
        }

        return announcements;
    }
}
