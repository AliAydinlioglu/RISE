using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.StudentActivities;

namespace Rise.Persistence.Configurations.StudentActivities;

internal class StudentActivitiesConfiguration  : EntityConfiguration<StudentActivity>
{
    public override void Configure(EntityTypeBuilder<StudentActivity> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Description);
        builder.Property(x => x.Date).IsRequired().HasColumnType("datetime");
        builder.Property(x => x.StartTime).IsRequired().HasColumnType("datetime");
        builder.Property(x => x.EndTime).IsRequired().HasColumnType("datetime");
        builder.Property(x => x.ImageUrl).HasMaxLength(2048);
        builder.HasOne(x => x.Location).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.StudentClub).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
    }
}