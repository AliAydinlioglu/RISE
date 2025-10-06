using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;
using Rise.Domain.Common;

namespace Rise.Persistence.Configurations.Calendar;

internal class SessionConfiguration: EntityConfiguration<Session>
{
    public override void Configure(EntityTypeBuilder<Session> builder)
    {
        base.Configure(builder);

        builder.Property(it => it.DayOfWeek)
            .IsRequired()
            .HasConversion<string>();
        
        builder.OwnsOne(it => it.TimeRange, timeRange =>
        {
            timeRange.Property(it => it.StartTime)
                .IsRequired()
                .HasColumnName(nameof(TimeRange.StartTime));
            
            timeRange.Property(it => it.EndTime)
                .IsRequired()
                .HasColumnName(nameof(TimeRange.EndTime));
        }).Navigation(it => it.TimeRange).IsRequired();

        builder.Property(it => it.Campus).IsRequired().HasMaxLength(250);
        builder.Property(it => it.Room).IsRequired().HasMaxLength(50);
    }
}