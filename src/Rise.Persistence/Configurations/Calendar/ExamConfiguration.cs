using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;

namespace Rise.Persistence.Configurations.Calendar;

internal class ExamConfiguration: EntityConfiguration<Exam>
{
    public override void Configure(EntityTypeBuilder<Exam> builder)
    {
        base.Configure(builder);

        builder.Property(it => it.Title).IsRequired().HasMaxLength(250);
        builder.Property(it => it.ExamTimestamp).IsRequired();
        builder.Property(it => it.Campus).IsRequired().HasMaxLength(250);
        builder.Property(it => it.Room).IsRequired().HasMaxLength(50);
    }
}