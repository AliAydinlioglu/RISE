namespace Rise.Domain.Calendar;

/// <summary>
/// Represents a defined academic semester within an academic year, including its duration and exam schedule.
/// </summary>
public class AcademicSemester : Entity
{
    public string AcademicYear { get; private set; }
    public SemesterType Type { get; private set; }
    public DateRange DateRange { get; private set; }
    public DateTimeOffset ExamStartDate { get; private set; }
    
    private AcademicSemester() {  }

    public AcademicSemester(string academicYear, SemesterType type, DateRange dateRange, DateTimeOffset? examStartDate = null)
    {
        AcademicYear = Guard.Against.NullOrWhiteSpace(academicYear);
        Type = type;
        DateRange = Guard.Against.Null(dateRange);
        
        Guard.Against.ExamStartDateRequiredUnlessEp3(type, examStartDate);
        ExamStartDate = examStartDate ?? dateRange.StartDate;
    }
}

public enum SemesterType
{
    Sem1 = 0,
    Sem2 = 1,
    Ep3 = 2
}

public static class AcademicSemesterGuards
{
    public static void ExamStartDateRequiredUnlessEp3(this IGuardClause guardClause, SemesterType type, DateTimeOffset? examStartDate)
    {
        if (type != SemesterType.Ep3 && examStartDate is null)
            throw new ArgumentException("ExamStartDate is required unless type is EP3");
    }
}
