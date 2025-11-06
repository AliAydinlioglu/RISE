using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Common;
using Rise.Domain.SchoolEvents;

namespace Rise.Persistence.Configurations.SchoolEvents
{
    internal class SchoolEventConfiguration : EntityConfiguration<SchoolEvent>
    {
        public override void Configure(EntityTypeBuilder<SchoolEvent> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description);
            builder.Property(x => x.Date).IsRequired().HasColumnType("datetime");
            builder.OwnsOne(it => it.TimeRange, timeRange =>
            {
                timeRange.Property(it => it.StartTime)
                    .IsRequired()
                    .HasColumnName(nameof(TimeRange.StartTime));

                timeRange.Property(it => it.EndTime)
                    .IsRequired()
                    .HasColumnName(nameof(TimeRange.EndTime));
            }).Navigation(it => it.TimeRange).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.RegisterLink).IsRequired().HasMaxLength(2048);
            builder.Property(x => x.Capacity).IsRequired();
            builder.Property(x => x.Registrable).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.Publicity).IsRequired().HasMaxLength(2048);
            builder.Property(x => x.ImageUrl).HasMaxLength(2048);
            builder.HasOne(x => x.Location).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
