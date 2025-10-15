using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Persistence.Configurations.Calendar;

internal class AcademicSemesterConfiguration: EntityConfiguration<AcademicSemester>
{
    public override void Configure(EntityTypeBuilder<AcademicSemester> builder)
    {
        base.Configure(builder);

        builder.Property(it => it.AcademicYear).IsRequired().HasMaxLength(9);
        builder.Property(it => it.Type).IsRequired().HasConversion<string>();
        
        builder.OwnsOne(it => it.DateRange, dateRange =>
        {
            dateRange.Property(it => it.StartDate).IsRequired().HasColumnName(nameof(DateRange.StartDate));
            dateRange.Property(it => it.EndDate).IsRequired().HasColumnName(nameof(DateRange.EndDate));
        }).Navigation(it => it.DateRange).IsRequired();

        builder.Property(it => it.ExamStartDate).IsRequired();
    }
}