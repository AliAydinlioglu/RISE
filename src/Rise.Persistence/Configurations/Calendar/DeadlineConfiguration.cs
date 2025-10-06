using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Calendar;

namespace Rise.Persistence.Configurations.Calendar;

internal class DeadlineConfiguration: EntityConfiguration<Deadline>
{
    public override void Configure(EntityTypeBuilder<Deadline> builder)
    {
        base.Configure(builder);
        
        builder.Property(it => it.TaskTitle).IsRequired().HasMaxLength(250);
        builder.Property(it => it.TaskDescription).HasMaxLength(1_000);
        builder.Property(it => it.DeadlineTimestamp).IsRequired();
    }
}