namespace Rise.Domain.Calendar;

public class Period : Entity
{
    public string AcademicYear { get; private set; }
    public PeriodType Type { get; private set; }
    public DateRange DateRange { get; private set; }
    public DateTimeOffset ExamStartDate { get; private set; }
    
    private Period() {  }

    public Period(string academicYear, PeriodType type, DateRange dateRange, DateTimeOffset? examStartDate = null)
    {
        AcademicYear = Guard.Against.NullOrWhiteSpace(academicYear);
        Type = type;
        DateRange = Guard.Against.Null(dateRange);
        
        Guard.Against.ExamStartDateRequiredUnlessEp3(type, examStartDate);
        ExamStartDate = examStartDate ?? dateRange.StartDate;
    }
}

public enum PeriodType
{
    Sem1 = 0,
    Sem2 = 1,
    Ep3 = 2
}

public static class PeriodGuards
{
    public static void ExamStartDateRequiredUnlessEp3(this IGuardClause guardClause, PeriodType type, DateTimeOffset? examStartDate)
    {
        if (type != PeriodType.Ep3 && examStartDate is null)
            throw new ArgumentException("ExamStartDate is required unless type is EP3");
    }
}
