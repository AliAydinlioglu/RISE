using Ardalis.Result;
using Rise.Shared.Calendar;

namespace Rise.Client.Calendar.Fakers;

public class FakeCalendarServiceWithoutDeadlines : ICalendarService
{
    public async Task<Result<CalendarResponse.Get>> GetCalendarAsync(string userId)
    {
        return await Task.FromResult(Result.Success(Response));
    }

    private static readonly CalendarResponse.Get Response = new()
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
                Deadlines = [],
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
                Exams =
                [
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