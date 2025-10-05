using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Calendar;

public class GivenAPeriod
{
    [Fact]
    public void WhenAcademicYearIsEmpty_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Period("", PeriodType.Sem1, AValidDateRange, AValidExamStartDate));
    }
    
    [Fact]
    public void WhenAcademicYearIsNull_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new Period(null!, PeriodType.Sem1, AValidDateRange, AValidExamStartDate));
    }

    [Fact]
    public void WhenDateRangeIsNull_ThenAnExceptionIsThrown()
    {
        var examStartDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        Should.Throw<ArgumentNullException>(() =>
            new Period("2024-2025", PeriodType.Sem1, null!, examStartDate));
    }
    
    [Fact]
    public void WhenEp3_ThenExamStartDateIsSemStartDate()
    {
        var period = new Period("2025-2026", PeriodType.Ep3, AValidDateRange);

        var expected = AValidDateRange.StartDate;
        period.ExamStartDate.ShouldBe(expected);
    }
    
    [Fact]
    public void WhenNotEp3AndExamDateMissing_ThenExamStartDateIsSemStartDate()
    {
        Should.Throw<ArgumentException>(() => 
            new Period("2025-2026", PeriodType.Sem2, AValidDateRange));
    }

    private static readonly DateRange AValidDateRange = new(
        new DateTimeOffset(2025, 9, 22, 0, 0, 0, TimeSpan.Zero),
        new DateTimeOffset(2025, 12, 14, 0, 0, 0, TimeSpan.Zero)
    );
    
    private static readonly DateTimeOffset AValidExamStartDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
}