using Rise.Shared.Calendar;

namespace Rise.TestDoubles;

public static class CalendarObjectMother
{
    public static CalendarResponse.Get BuildGetResponse(bool withDeadlines = true)
    {
        var response = new CalendarResponse.Get
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
                    Lecturer = new CalendarResponse.LecturerInfo
                    {
                        FirstName = "Alice",
                        LastName = "Alisson",
                    },
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
                    Deadlines = withDeadlines 
                        ? [
                            new CalendarResponse.DeadlineInfo
                            {
                                DeadlineId = "1",
                                TaskTitle = "Deadline RISE",
                                TaskDescription = "CampusApp Demo",
                                DeadlineTimestamp = new DateTimeOffset(2024, 11, 12, 23, 59, 59, TimeSpan.Zero)
                            }
                        ]
                        : [],
                    Exams = []
                },
                new CalendarResponse.CourseInfo
                {
                    CourseId = "2",
                    CourseTitle = "Modern Data Structures",
                    Lecturer = new CalendarResponse.LecturerInfo
                    {
                        FirstName = "Bob",
                        LastName = "Bobson",
                    },
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

        return response;
    }
}