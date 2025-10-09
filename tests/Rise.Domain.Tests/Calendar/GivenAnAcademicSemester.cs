using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Domain.Tests.Calendar;

public class GivenAnAcademicSemester
{
    [Fact]
    public void WhenAcademicYearIsEmpty_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new AcademicSemester("", SemesterType.Sem1, AValidDateRange, AValidExamStartDate));
    }
    
    [Fact]
    public void WhenAcademicYearIsNull_ThenAnExceptionIsThrown()
    {
        Should.Throw<ArgumentException>(() =>
            new AcademicSemester(null!, SemesterType.Sem1, AValidDateRange, AValidExamStartDate));
    }

    [Fact]
    public void WhenDateRangeIsNull_ThenAnExceptionIsThrown()
    {
        var examStartDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        Should.Throw<ArgumentNullException>(() =>
            new AcademicSemester("2024-2025", SemesterType.Sem1, null!, examStartDate));
    }
    
    [Fact]
    public void WhenEp3_ThenExamStartDateIsSemStartDate()
    {
        var academicSemester = new AcademicSemester("2025-2026", SemesterType.Ep3, AValidDateRange);

        var expected = AValidDateRange.StartDate;
        academicSemester.ExamStartDate.ShouldBe(expected);
    }
    
    [Fact]
    public void WhenNotEp3AndExamDateMissing_ThenExamStartDateIsSemStartDate()
    {
        Should.Throw<ArgumentException>(() => 
            new AcademicSemester("2025-2026", SemesterType.Sem2, AValidDateRange));
    }

    private static readonly DateRange AValidDateRange = new(
        new DateTimeOffset(2025, 9, 22, 0, 0, 0, TimeSpan.Zero),
        new DateTimeOffset(2025, 12, 14, 0, 0, 0, TimeSpan.Zero)
    );
    
    private static readonly DateTimeOffset AValidExamStartDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
}