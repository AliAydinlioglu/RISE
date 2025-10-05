namespace Rise.Shared.Calendar;

public static class CalendarResponse
{
    public class Get
    {
        public string ClassGroup { get; set; }
        public string AcademicYear { get; set; }
        public PeriodInfo Period { get; set; }
        public List<CourseInfo> Courses { get; set; } = new();
    }

    public class PeriodInfo
    {
        public string PeriodId { get; set; }
        public string Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset ExamStartDate { get; set; }
    }

    public class CourseInfo
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string Lecturer { get; set; }
        public List<SessionInfo> Sessions { get; set; } = new();
        public List<DeadlineInfo> Deadlines { get; set; } = new();
        public List<ExamInfo> Exams { get; set; } = new();
    }

    public class SessionInfo
    {
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Campus { get; set; }
        public string Room { get; set; }
    }

    public class DeadlineInfo
    {
        public string DeadlineId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTimeOffset DeadlineTimestamp { get; set; }
    }

    public class ExamInfo
    {
        public string ExamId { get; set; }
        public string ExamTitle { get; set; }
        public DateTimeOffset ExamTimestamp { get; set; }
        public string Campus { get; set; }
        public string Room { get; set; }
    }
}