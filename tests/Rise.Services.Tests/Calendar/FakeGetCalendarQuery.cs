using Ardalis.Result;
using Rise.Shared.Calendar;

namespace Rise.Services.Tests.Calendar;

public class FakeGetCalendarQuery: IGetCalendarQuery
{
    public Task<Result<CalendarResponse.Get>> ExecuteAsync(string userClassGroup)
    {
        var calendar = new CalendarResponse.Get
        {
            ClassGroup = userClassGroup,
            AcademicYear = "2024-2025",
            AcademicSemester = new CalendarResponse.AcademicSemesterInfo
            {
                AcademicSemesterId = "7803f803-c2cf-40f9-8c64-9daa5f2e9c87",
                Type = "SEM1",
                StartDate = DateTimeOffset.Parse("2025-09-22T00:00:00+00:00"),
                EndDate = DateTimeOffset.Parse("2025-12-14T00:00:00+00:00"),
                ExamStartDate = DateTimeOffset.Parse("2026-01-01T00:00:00+00:00")
            },
            Courses = new List<CalendarResponse.CourseInfo>
            {
                new()
                {
                    CourseId = "37eff00d-3430-4c05-b7e5-bae3334aa7c9",
                    CourseTitle = "RISE",
                    Lecturer = "Alice",
                    Lessons = new List<CalendarResponse.LessonInfo>
                    {
                        new() { Day = "MONDAY", StartTime = "08:30", EndTime = "10:30", Campus = "Schoonmeersen", Room = "GSCHB.2.001" },
                        new() { Day = "THURSDAY", StartTime = "10:45", EndTime = "13:00", Campus = "Schoonmeersen", Room = "GSCHB.2.001" }
                    }
                },
                new()
                {
                    CourseId = "11bc4392-d0d7-4d55-b5b1-8a76bdcd68a2",
                    CourseTitle = "FALL",
                    Lecturer = "Bob",
                    Lessons = new List<CalendarResponse.LessonInfo>
                    {
                        new() { Day = "WEDNESDAY", StartTime = "14:30", EndTime = "17:30", Campus = "Schoonmeersen", Room = "GSCHT.1.101" }
                    },
                    Deadlines = new List<CalendarResponse.DeadlineInfo>
                    {
                        new() { DeadlineId = "87ac397b-cc95-4baa-8c02-9adaec280ae1", TaskTitle = "Campus App", TaskDescription = "Description", DeadlineTimestamp = DateTimeOffset.Parse("2025-12-14T23:59:59+00:00") }
                    },
                    Exams = new List<CalendarResponse.ExamInfo>
                    {
                        new() { ExamId = "c718d6a4-e9a9-41ae-bc7d-b377b6e92697", ExamTitle = "Fall - Theorie", ExamTimestamp = DateTimeOffset.Parse("2025-01-10T09:12:30+00:00"), Campus = "Schoonmeersen", Room = "GSCHT.1.101" },
                        new() { ExamId = "c718d6a4-e9a9-41ae-bc7d-b377b6e92697", ExamTitle = "Fall - Praktijk", ExamTimestamp = DateTimeOffset.Parse("2025-01-10T09:15:00+00:00"), Campus = "Schoonmeersen", Room = "GSCHT.1.101" }
                    }
                }
            }
        };
        
        return Task.FromResult(Result.Success(calendar));
    }
}